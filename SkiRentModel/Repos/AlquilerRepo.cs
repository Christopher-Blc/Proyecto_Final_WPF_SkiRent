/* 
Pseudocodigo detallado:
- Añadir comentarios XML simples en espanol sin acentos ni tecnicismos.
- Comentar la clase y cada metodo publico con <summary>.
- Para metodos con parametros usar <param name="..."> y explicar en palabras simples.
- Para metodos que devuelven valor usar <returns> con descripcion simple.
- Colocar los comentarios sobre las firmas actuales sin cambiar la logica del codigo.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para manejar las operaciones sobre alquileres.
    /// Aqui se guardan, buscan, editan y borran alquileres.
    /// </summary>
    public class AlquilerRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve todos los alquileres almacenados.
        /// </summary>
        /// <returns>Lista con todos los alquileres.</returns>
        public List<Alquiler> Listar()
        {
            return _context.Alquiler.ToList();
        }

        /// <summary>
        /// Busca un alquiler por su id.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler a buscar.</param>
        /// <returns>El alquiler encontrado o null si no existe.</returns>
        public Alquiler BuscarPorId(int idAlquiler)
        {
            return _context.Alquiler.Find(idAlquiler);
        }

        /// <summary>
        /// Busca alquileres que coincidan con el texto.
        /// Si el texto esta vacio devuelve todos.
        /// </summary>
        /// <param name="texto">Texto para buscar por estado o id cliente.</param>
        /// <returns>Lista de alquileres que coinciden con la busqueda.</returns>
        public List<Alquiler> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Listar();

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
        /// Anyade un nuevo alquiler al sistema.
        /// </summary>
        /// <param name="alquiler">Objeto alquiler a guardar.</param>
        public void Anyadir(Alquiler alquiler)
        {
            _context.Alquiler.Add(alquiler);
            _context.SaveChanges();
        }

        /// <summary>
        /// Edita un alquiler existente con los datos dados.
        /// Si no existe no hace nada.
        /// </summary>
        /// <param name="alquilerActualizado">Alquiler con los datos actualizados. Debe tener el Id correcto.</param>
        public void Editar(Alquiler alquilerActualizado)
        {
            var alquilerBD = _context.Alquiler.Find(alquilerActualizado.IdAlquiler);
            if (alquilerBD == null) return;

            alquilerBD.IdCliente = alquilerActualizado.IdCliente;
            alquilerBD.FechaInicio = alquilerActualizado.FechaInicio;
            alquilerBD.FechaFin = alquilerActualizado.FechaFin;
            alquilerBD.Estado = alquilerActualizado.Estado;

            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina un alquiler y sus lineas relacionadas.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler a eliminar.</param>
        /// <returns>True si se elimino, false si no se encontro.</returns>
        public bool Eliminar(int idAlquiler)
        {
            var alquilerBD = _context.Alquiler.Find(idAlquiler);
            if (alquilerBD == null)
            {
                return false;
            }

            //borrar dependientes primero
            var lineas = _context.LineaAlquiler.Where(l => l.IdAlquiler == idAlquiler).ToList();
            if (lineas.Count > 0)
            {
                _context.LineaAlquiler.RemoveRange(lineas);
            }

            _context.Alquiler.Remove(alquilerBD);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Cuenta cuantos alquileres tienen estado abierto.
        /// </summary>
        /// <returns>Numero de alquileres con estado "Abierto".</returns>
        public int CantidadActivos()
        {
            return _context.Alquiler.Count(a => a.Estado == "Abierto");
        }

        /// <summary>
        /// Indica si un alquiler tiene lineas asociadas.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler a comprobar.</param>
        /// <returns>True si tiene lineas, false si no.</returns>
        public bool TieneLineas(int idAlquiler)
        {
            return _context.LineaAlquiler.Any(l => l.IdAlquiler == idAlquiler);
        }
    }
}
