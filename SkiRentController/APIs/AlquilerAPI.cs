using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Clase que se usa desde la interfaz para trabajar con alquileres.
    /// Llama al repositorio y aplica validaciones antes de guardar.
    /// </summary>
    public class AlquilerAPI
    {
        /// <summary>
        /// Repositorio que realiza las operaciones en la base de datos.
        /// </summary>
        private AlquilerRepo repo = new AlquilerRepo();

        /// <summary>
        /// Devuelve todos los alquileres guardados.
        /// </summary>
        /// <returns>Lista con todos los alquileres.</returns>
        public List<Alquiler> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca un alquiler por su id.
        /// </summary>
        /// <param name="id">Id del alquiler que se quiere buscar.</param>
        /// <returns>El alquiler encontrado, o null si no existe.</returns>
        public Alquiler BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Busca alquileres segun un texto.
        /// Si el texto esta vacio, devuelve todos los alquileres.
        /// </summary>
        /// <param name="texto">Texto para buscar por estado o por id de cliente.</param>
        /// <returns>Lista de alquileres que coinciden con la busqueda.</returns>
        public List<Alquiler> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        /// <summary>
        /// Crea un alquiler nuevo con los datos indicados.
        /// Si hay un error de datos, devuelve un mensaje.
        /// </summary>
        /// <param name="idCliente">Id del cliente del alquiler.</param>
        /// <param name="fechaInicio">Fecha de inicio del alquiler.</param>
        /// <param name="fechaFin">Fecha de fin del alquiler.</param>
        /// <param name="estado">Estado del alquiler, por ejemplo Abierto.</param>
        /// <returns>Null si se creo bien, o un mensaje si hubo error.</returns>
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
        /// Edita un alquiler existente con los datos indicados.
        /// Si hay un error de datos, devuelve un mensaje.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler que se quiere editar.</param>
        /// <param name="idCliente">Id del cliente del alquiler.</param>
        /// <param name="fechaInicio">Nueva fecha de inicio.</param>
        /// <param name="fechaFin">Nueva fecha de fin.</param>
        /// <param name="estado">Nuevo estado del alquiler.</param>
        /// <returns>Null si se edito bien, o un mensaje si hubo error.</returns>
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
        /// <param name="idAlquiler">Id del alquiler que se quiere eliminar.</param>
        /// <returns>Null si se elimino bien, o un mensaje si hubo error.</returns>
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
        /// <returns>Numero de alquileres con estado Abierto.</returns>
        public int CantidadActivos()
        {
            return repo.CantidadActivos();
        }
    }
}
