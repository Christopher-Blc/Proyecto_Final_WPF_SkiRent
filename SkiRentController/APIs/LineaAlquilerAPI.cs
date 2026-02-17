using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Api para gestionar las lineas de un alquiler desde la interfaz.
    /// Aqui se validan datos y se aplican reglas como stock y estado.
    /// </summary>
    public class LineaAlquilerAPI
    {
        /// <summary>
        /// Repositorio para trabajar con lineas de alquiler.
        /// </summary>
        private LineaAlquilerRepo repo = new LineaAlquilerRepo();

        /// <summary>
        /// Repositorio para trabajar con materiales.
        /// </summary>
        private MaterialRepo materialRepo = new MaterialRepo();

        /// <summary>
        /// Repositorio para trabajar con alquileres.
        /// </summary>
        private AlquilerRepo alquilerRepo = new AlquilerRepo();

        /// <summary>
        /// Devuelve las lineas de un alquiler.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler.</param>
        /// <returns>Lista de lineas del alquiler.</returns>
        public List<LineaAlquiler> ListarPorAlquiler(int idAlquiler)
        {
            return repo.ListarPorAlquiler(idAlquiler);
        }

        /// <summary>
        /// Devuelve los materiales disponibles con stock.
        /// </summary>
        /// <returns>Lista de materiales que se pueden anadir.</returns>
        public List<Material> ListarMaterialesDisponibles()
        {
            return materialRepo.Listar()
                .Where(m => m.Estado == "Disponible" && m.Stock > 0)
                .ToList();
        }

        /// <summary>
        /// Valida los datos y anade una linea al alquiler si se puede.
        /// Descuenta el stock y actualiza el total del alquiler.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler.</param>
        /// <param name="idMaterial">Id del material.</param>
        /// <param name="cantidad">Cantidad a anadir.</param>
        /// <param name="dias">Dias del alquiler para esa linea.</param>
        /// <returns>Null si fue bien, o un mensaje si hubo error.</returns>
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
                return "Solo se pueden anadir productos si el alquiler esta Abierto.";
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

            LineaAlquiler linea = new LineaAlquiler
            {
                IdAlquiler = idAlquiler,
                IdMaterial = idMaterial,
                Cantidad = cantidad,
                Dias = dias,
                PrecioDiaAplicado = material.PrecioDia,
                Subtotal = material.PrecioDia * dias * cantidad
            };

            material.Stock -= cantidad;
            materialRepo.Editar(material);

            repo.Anyadir(linea);
            repo.ActualizarTotal(idAlquiler);

            return null;
        }

        /// <summary>
        /// Elimina una linea si se puede.
        /// Devuelve el stock y actualiza el total del alquiler.
        /// </summary>
        /// <param name="idLinea">Id de la linea.</param>
        /// <returns>Null si fue bien, o un mensaje si hubo error.</returns>
        public string EliminarLinea(int idLinea)
        {
            if (idLinea <= 0)
            {
                return "Linea no valida.";
            }

            var linea = repo.BuscarPorId(idLinea);
            if (linea == null)
            {
                return "La linea no existe.";
            }

            var alquiler = alquilerRepo.BuscarPorId(linea.IdAlquiler);
            if (alquiler == null)
            {
                return "El alquiler no existe.";
            }

            if (alquiler.Estado != "Abierto")
            {
                return "No se puede eliminar la linea si el alquiler no esta Abierto.";
            }

            var material = materialRepo.BuscarPorId(linea.IdMaterial);
            if (material != null)
            {
                material.Stock += linea.Cantidad;
                materialRepo.Editar(material);
            }

            int idAlquiler = linea.IdAlquiler;

            repo.Eliminar(linea);
            repo.ActualizarTotal(idAlquiler);

            return null;
        }
    }
}
