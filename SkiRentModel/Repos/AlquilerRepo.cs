using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para trabajar con alquileres en la base de datos.
    /// Permite listar, buscar, anadir, editar y eliminar alquileres.
    /// </summary>
    public class AlquilerRepo
    {
        /// <summary>
        /// Contexto de base de datos que se usa para leer y guardar datos.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve todos los alquileres guardados.
        /// </summary>
        /// <returns>Lista con todos los alquileres.</returns>
        public List<Alquiler> Listar()
        {
            return _context.Alquiler.ToList();
        }

        /// <summary>
        /// Busca un alquiler por su id.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler que se quiere buscar.</param>
        /// <returns>El alquiler encontrado, o null si no existe.</returns>
        public Alquiler BuscarPorId(int idAlquiler)
        {
            return _context.Alquiler.Find(idAlquiler);
        }

        /// <summary>
        /// Busca alquileres por un texto.
        /// Si el texto esta vacio, devuelve todos los alquileres.
        /// </summary>
        /// <param name="texto">Texto para buscar por estado o por id de cliente.</param>
        /// <returns>Lista de alquileres que coinciden con la busqueda.</returns>
        public List<Alquiler> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return Listar();
            }

            texto = texto.Trim();

            int idCliente;
            bool esNumero = int.TryParse(texto, out idCliente);

            return _context.Alquiler
                .Where(a =>
                    a.Estado.Contains(texto) ||
                    (esNumero && a.IdCliente == idCliente)
                )
                .ToList();
        }

        /// <summary>
        /// Anade un alquiler nuevo a la base de datos.
        /// </summary>
        /// <param name="alquiler">Alquiler que se quiere guardar.</param>
        public void Anyadir(Alquiler alquiler)
        {
            _context.Alquiler.Add(alquiler);
            _context.SaveChanges();
        }

        /// <summary>
        /// Edita un alquiler que ya existe.
        /// Si no existe, no hace nada.
        /// </summary>
        /// <param name="alquilerActualizado">Alquiler con los datos nuevos y el id correcto.</param>
        public void Editar(Alquiler alquilerActualizado)
        {
            var alquilerBD = _context.Alquiler.Find(alquilerActualizado.IdAlquiler);
            if (alquilerBD == null)
            {
                return;
            }

            alquilerBD.IdCliente = alquilerActualizado.IdCliente;
            alquilerBD.FechaInicio = alquilerActualizado.FechaInicio;
            alquilerBD.FechaFin = alquilerActualizado.FechaFin;
            alquilerBD.Estado = alquilerActualizado.Estado;

            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina un alquiler por su id.
        /// Antes borra las lineas del alquiler para evitar errores.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler que se quiere eliminar.</param>
        /// <returns>True si se elimino, false si no se encontro.</returns>
        public bool Eliminar(int idAlquiler)
        {
            var alquilerBD = _context.Alquiler.Find(idAlquiler);
            if (alquilerBD == null)
            {
                return false;
            }

            // Borra primero las lineas relacionadas con este alquiler
            var lineas = _context.LineaAlquiler
                                 .Where(l => l.IdAlquiler == idAlquiler)
                                 .ToList();

            if (lineas.Count > 0)
            {
                _context.LineaAlquiler.RemoveRange(lineas);
            }

            _context.Alquiler.Remove(alquilerBD);
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Cuenta cuantos alquileres estan en estado Abierto.
        /// </summary>
        /// <returns>Numero de alquileres con estado Abierto.</returns>
        public int CantidadActivos()
        {
            return _context.Alquiler.Count(a => a.Estado == "Abierto");
        }

        /// <summary>
        /// Dice si un alquiler tiene lineas asociadas.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler a comprobar.</param>
        /// <returns>True si tiene lineas, false si no tiene.</returns>
        public bool TieneLineas(int idAlquiler)
        {
            return _context.LineaAlquiler.Any(l => l.IdAlquiler == idAlquiler);
        }
    }
}
