using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Clase que gestiona las operaciones sobre las lineas de un alquiler.
    /// </summary>
    public class LineaAlquilerAPI
    {
        /// <summary>
        /// Repositorio para manejar las lineas de alquiler en la base de datos.
        /// </summary>
        private LineaAlquilerRepo repo = new LineaAlquilerRepo();

        /// <summary>
        /// Repositorio para manejar los materiales.
        /// </summary>
        private MaterialRepo materialRepo = new MaterialRepo();

        /// <summary>
        /// Repositorio para manejar los alquileres.
        /// </summary>
        private AlquilerRepo alquilerRepo = new AlquilerRepo();

        /// <summary>
        /// Devuelve la lista de lineas que pertenecen a un alquiler dado.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler cuyas lineas se quieren listar.</param>
        /// <returns>Lista de LineaAlquiler; puede estar vacia si no hay lineas.</returns>
        public List<LineaAlquiler> ListarPorAlquiler(int idAlquiler)
        {
            return repo.ListarPorAlquiler(idAlquiler);
        }                                       

        /// <summary>
        /// Lista los materiales que estan disponibles y tienen stock.
        /// </summary>
        /// <returns>Lista de Material que se pueden anadir al alquiler.</returns>
        public List<Material> ListarMaterialesDisponibles()
        {
            return materialRepo.Listar()
                .Where(m => m.Estado == "Disponible" && m.Stock > 0)
                .ToList();
        }

        /// <summary>
        /// Valida los datos y anade una linea al alquiler si todo es correcto.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler donde se quiere anadir la linea.</param>
        /// <param name="idMaterial">Id del material a anadir.</param>
        /// <param name="cantidad">Cantidad de unidades a anadir.</param>
        /// <param name="dias">Numero de dias para calcular el subtotal.</param>
        /// <returns>
        /// Null si la operacion fue correcta; texto con el error si fallo alguna validacion.
        /// </returns>
        public string AnyadirLinea(int idAlquiler, int idMaterial, int cantidad, int dias)
        {
            string error = Validaciones.ValidarLineaAlquiler(idAlquiler, idMaterial, cantidad, dias);
            if (error != null)
            {
                return error;
            }

            var alquiler = alquilerRepo.BuscarPorId(idAlquiler);
            if (alquiler == null)
            {
                return "El alquiler no existe.";
            }

            if (alquiler.Estado != "Abierto")
            {
                return "Solo se pueden añadir productos si el alquiler esta Abierto.";
            }

            var material = materialRepo.BuscarPorId(idMaterial);
            if (material == null)
            {
                return "El material no existe.";
            }

            if (material.Estado != "Disponible")
            {
                return "El material no esta disponible.";
            }

            if (material.Stock < cantidad)
            {
                return "No hay stock suficiente.";
            }

            // Creamos la linea , PrecioDiaAplicado y Subtotal los calcula el repo
            LineaAlquiler linea = new LineaAlquiler
            {
                IdAlquiler = idAlquiler,
                IdMaterial = idMaterial,
                Cantidad = cantidad,
                Dias = dias,
                PrecioDiaAplicado = material.PrecioDia
            };

            bool anyadirCorrecto = repo.Anyadir(linea);
            if (!anyadirCorrecto)
            {
                return "No se ha podido añadir la línea. Revisa que el alquiler esté Abierto y quede stock.";
            }

            return null;
        }

        /// <summary>
        /// Elimina una linea de alquiler si el id es valido y la operacion se puede realizar.
        /// </summary>
        /// <param name="idLinea">Id de la linea que se quiere eliminar.</param>
        /// <returns>
        /// Null si se elimino correctamente; texto con el error si no se pudo eliminar.
        /// </returns>
        public string EliminarLinea(int idLinea)
        {
            if (idLinea <= 0)
            {
                return "Linea no valida.";
            }

            bool eliminarCorrecto = repo.Eliminar(idLinea);
            if (!eliminarCorrecto)
            {
                return "No se ha podido eliminar la linea. Solo se puede eliminar si el alquiler esta Abierto";
            }

            return null;
        }
    }
}
