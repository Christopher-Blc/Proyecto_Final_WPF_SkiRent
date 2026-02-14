using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_WPF_SkiRent.Controllers
{
    public class LineaAlquilerAPI
    {
        private LineaAlquilerRepo repo = new LineaAlquilerRepo();
        private MaterialRepo materialRepo = new MaterialRepo();
        private AlquilerRepo alquilerRepo = new AlquilerRepo();

        public List<LineaAlquiler> ListarPorAlquiler(int idAlquiler)
        {
            return repo.ListarPorAlquiler(idAlquiler);
        }

        //para llenar el cmb de materiales en la parte derecha
        //solo vamos a enseñar los materiales con estado disponible y que tengan stock
        public List<Material> ListarMaterialesDisponibles()
        {
            return materialRepo.Listar()
                .Where(m => m.Estado == "Disponible" && m.Stock > 0)
                .ToList();
        }

        //Añadir una linea al alquiler
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
