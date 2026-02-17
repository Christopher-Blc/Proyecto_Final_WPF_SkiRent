using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para trabajar con lineas de alquiler en la base de datos.
    /// Contiene operaciones de lectura y guardado.
    /// </summary>
    public class LineaAlquilerRepo
    {
        /// <summary>
        /// Contexto de base de datos usado para leer y guardar lineas.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve las lineas que pertenecen a un alquiler.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler.</param>
        /// <returns>Lista de lineas del alquiler.</returns>
        public List<LineaAlquiler> ListarPorAlquiler(int idAlquiler)
        {
            return _context.LineaAlquiler
                .Where(l => l.IdAlquiler == idAlquiler)
                .ToList();
        }

        /// <summary>
        /// Busca una linea por su id.
        /// </summary>
        /// <param name="idLinea">Id de la linea.</param>
        /// <returns>Linea encontrada o null.</returns>
        public LineaAlquiler BuscarPorId(int idLinea)
        {
            return _context.LineaAlquiler.Find(idLinea);
        }

        /// <summary>
        /// Guarda una linea nueva en la base de datos.
        /// </summary>
        /// <param name="linea">Linea a guardar.</param>
        public void Anyadir(LineaAlquiler linea)
        {
            _context.LineaAlquiler.Add(linea);
            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina una linea existente.
        /// </summary>
        /// <param name="linea">Linea a eliminar.</param>
        public void Eliminar(LineaAlquiler linea)
        {
            _context.LineaAlquiler.Remove(linea);
            _context.SaveChanges();
        }

        /// <summary>
        /// Recalcula el total del alquiler sumando los subtotales de sus lineas.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler.</param>
        public void ActualizarTotal(int idAlquiler)
        {
            var alquiler = _context.Alquiler.Find(idAlquiler);
            if (alquiler == null)
            {
                return;
            }

            var total = _context.LineaAlquiler
                .Where(l => l.IdAlquiler == idAlquiler)
                .Select(l => (decimal?)l.Subtotal)
                .Sum() ?? 0m;

            alquiler.Total = total;
            _context.SaveChanges();
        }

        
    }
}
