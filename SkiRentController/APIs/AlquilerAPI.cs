using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// API para manejar operaciones sobre alquileres desde la capa de presentacion.
    /// Proporciona metodos para listar, buscar, crear, editar y eliminar alquileres.
    /// </summary>
        public class AlquilerAPI
        {
        /// <summary>
        /// Repositorio usado para realizar las operaciones de datos sobre alquileres.
        /// </summary>
        private AlquilerRepo repo = new AlquilerRepo();

        /// <summary>
        /// Devuelve todos los alquileres almacenados.
        /// </summary>
        /// <returns>Lista con todos los alquileres.</returns>
        public List<Alquiler> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca un alquiler por su id.
        /// </summary>
        /// <param name="id">Id del alquiler a buscar.</param>
        /// <returns>El alquiler encontrado o null si no existe.</returns>
        public Alquiler BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Busca alquileres que coincidan con el texto dado.
        /// Si el texto esta vacio devuelve todos los alquileres.
        /// </summary>
        /// <param name="texto">Texto para buscar por estado o id cliente.</param>
        /// <returns>Lista de alquileres que coinciden con la busqueda.</returns>
        public List<Alquiler> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        /// <summary>
        /// Crea un nuevo alquiler con los datos dados.
        /// Valida los datos antes de guardarlo.
        /// </summary>
        /// <param name="idCliente">Id del cliente que realiza el alquiler.</param>
        /// <param name="fechaInicio">Fecha de inicio del alquiler.</param>
        /// <param name="fechaFin">Fecha de fin del alquiler.</param>
        /// <param name="estado">Estado inicial del alquiler.</param>
        /// <returns>
        /// Null si la creacion fue correcta; en caso de error devuelve un mensaje con la causa.
        /// </returns>
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
        /// Edita un alquiler existente with los datos proporcionados.
        /// Valida los datos y comprueba que el id del alquiler sea valido.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler a editar.</param>
        /// <param name="idCliente">Id del cliente asociado al alquiler.</param>
        /// <param name="fechaInicio">Nueva fecha de inicio.</param>
        /// <param name="fechaFin">Nueva fecha de fin.</param>
        /// <param name="estado">Nuevo estado del alquiler.</param>
        /// <returns>
        /// Null si la edicion fue correcta; en caso de error devuelve un mensaje con la causa.
        /// </returns>
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
        /// <param name="idAlquiler">Id del alquiler a eliminar.</param>
        /// <returns>
        /// Null si la eliminacion fue correcta; en caso de error devuelve un mensaje.
        /// </returns>
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
        /// Cuenta cuantos alquileres estan en estado abierto.
        /// </summary>
        /// <returns>Numero de alquileres con estado abierto.</returns>
        public int CantidadActivos()
        {
            return repo.CantidadActivos();
        }
    }
}
