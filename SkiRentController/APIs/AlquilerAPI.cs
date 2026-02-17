using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Api para trabajar con alquileres desde la interfaz.
    /// Valida datos y llama al repositorio para guardar cambios.
    /// </summary>
    public class AlquilerAPI
    {
        /// <summary>
        /// Repositorio usado para leer y guardar alquileres.
        /// </summary>
        private AlquilerRepo repo = new AlquilerRepo();

        /// <summary>
        /// Devuelve todos los alquileres.
        /// </summary>
        /// <returns>Lista con todos los alquileres.</returns>
        public List<Alquiler> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca un alquiler por su id.
        /// </summary>
        /// <param name="id">Id del alquiler.</param>
        /// <returns>Alquiler si existe, o null si no existe.</returns>
        public Alquiler BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Busca alquileres segun un texto.
        /// Si el texto esta vacio devuelve todos.
        /// </summary>
        /// <param name="texto">Texto para buscar por estado o id de cliente.</param>
        /// <returns>Lista de alquileres que coinciden.</returns>
        public List<Alquiler> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        /// <summary>
        /// Crea un alquiler nuevo.
        /// </summary>
        /// <param name="idCliente">Id del cliente.</param>
        /// <param name="fechaInicio">Fecha de inicio.</param>
        /// <param name="fechaFin">Fecha de fin.</param>
        /// <param name="estado">Estado del alquiler.</param>
        /// <returns>Null si fue bien, o un mensaje si hubo error.</returns>
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

        /// <summary>
        /// Edita un alquiler existente.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler.</param>
        /// <param name="idCliente">Id del cliente.</param>
        /// <param name="fechaInicio">Nueva fecha inicio.</param>
        /// <param name="fechaFin">Nueva fecha fin.</param>
        /// <param name="estado">Nuevo estado.</param>
        /// <returns>Null si fue bien, o un mensaje si hubo error.</returns>
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

        /// <summary>
        /// Elimina un alquiler por su id.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler.</param>
        /// <returns>Null si fue bien, o un mensaje si hubo error.</returns>
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

        /// <summary>
        /// Cuenta cuantos alquileres estan en estado Abierto.
        /// </summary>
        /// <returns>Numero de alquileres abiertos.</returns>
        public int CantidadActivos()
        {
            return repo.CantidadActivos();
        }
    }
}
