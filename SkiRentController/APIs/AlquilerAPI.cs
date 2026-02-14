using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    public class AlquilerAPI
    {
        private AlquilerRepo repo = new AlquilerRepo();

        public List<Alquiler> Listar()
        {
            return repo.Listar();
        }

        public Alquiler BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        public List<Alquiler> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        public string Crear(int idCliente, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            string error = Validaciones.ValidarAlquiler(idCliente, fechaInicio, fechaFin, estado);

            if (error != null)
            {
                return error;
            }

            Alquiler a = new Alquiler
            {
                IdCliente = idCliente,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estado = estado.Trim(),
                Total = 0
            };

            repo.Anyadir(a);
            return null;
        }

        public string Editar(int idAlquiler, int idCliente, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            if (idAlquiler <= 0)
            {
                return "Alquiler no valido.";
            }

            string error = Validaciones.ValidarAlquiler(idCliente, fechaInicio, fechaFin, estado);

            if (error != null)
            {
                return error;
            }

            Alquiler a = new Alquiler
            {
                IdAlquiler = idAlquiler,
                IdCliente = idCliente,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estado = estado.Trim()
            };

            repo.Editar(a);
            return null;
        }

        public string Eliminar(int idAlquiler)
        {
            if (idAlquiler <= 0)
            {
                return "Alquiler no valido.";
            }


            bool eliminado = repo.Eliminar(idAlquiler);

            if (!eliminado)
            {
                return "Error al eliminar alquiler.";
            }

            return null;
        }

        public int CantidadActivos()
        {
            return repo.CantidadActivos();
        }
    }
}
