using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repo para manejar las lineas de alquiler en la base de datos.
    /// </summary>
    public class LineaAlquilerRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Lista las lineas que pertenecen a un alquiler dado.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler para filtrar las lineas.</param>
        /// <returns>Lista de lineas del alquiler. Puede estar vacia si no hay lineas.</returns>
        public List<LineaAlquiler> ListarPorAlquiler(int idAlquiler)
        {                                                                                               
            return _context.LineaAlquiler
                           .Where(l => l.IdAlquiler == idAlquiler)
                           .ToList();
        }

        /// <summary>
        /// Anade una linea al alquiler si el alquiler esta abierto.
        /// </summary>
        /// <param name="linea">Objeto linea con los datos a anadir (IdAlquiler, IdMaterial, Cantidad, Dias, PrecioDiaAplicado opcional).</param>
        /// <returns>true si se anadio la linea y se actualizo el stock y total; false si fallo alguna condicion.</returns>
        public bool Anyadir(LineaAlquiler linea)
        {
            var alquiler = _context.Alquiler.Find(linea.IdAlquiler);
            if (alquiler == null) return false;
            if (alquiler.Estado != "Abierto") return false;

            var material = _context.Material.Find(linea.IdMaterial);
            if (material == null) return false;

            if (material.Stock < linea.Cantidad) return false;

            if (linea.PrecioDiaAplicado <= 0)
                linea.PrecioDiaAplicado = material.PrecioDia;

            linea.Subtotal = linea.PrecioDiaAplicado * linea.Dias * linea.Cantidad;

            material.Stock -= linea.Cantidad;

            _context.LineaAlquiler.Add(linea);
            _context.SaveChanges();

            ActualizarTotal(linea.IdAlquiler);

            return true;
        }

        /// <summary>
        /// Elimina una linea de un alquiler si el alquiler esta abierto.
        /// </summary>
        /// <param name="idLinea">Id de la linea a eliminar.</param>
        /// <returns>true si se elimino y se devolvio el stock y se actualizo el total; false si no se pudo eliminar.</returns>
        public bool Eliminar(int idLinea)
        {
            var linea = _context.LineaAlquiler.Find(idLinea);
            if (linea == null) return false;

            var alquiler = _context.Alquiler.Find(linea.IdAlquiler);
            if (alquiler == null) return false;
            if (alquiler.Estado != "Abierto") return false;

            var material = _context.Material.Find(linea.IdMaterial);
            if (material != null)
                material.Stock += linea.Cantidad;

            int idAlquiler = linea.IdAlquiler;

            _context.LineaAlquiler.Remove(linea);
            _context.SaveChanges();

            ActualizarTotal(idAlquiler);

            return true;
        }

        /// <summary>
        /// Recalcula y actualiza el total del alquiler sumando los subtotales de sus lineas.
        /// </summary>
        /// <param name="idAlquiler">Id del alquiler cuyo total se debe recalcular.</param>
        private void ActualizarTotal(int idAlquiler)
        {
            var alquiler = _context.Alquiler.Find(idAlquiler);
            if (alquiler == null) return;

            var lineas = _context.LineaAlquiler
                                 .Where(l => l.IdAlquiler == idAlquiler)
                                 .ToList();

            alquiler.Total = lineas.Sum(l => l.Subtotal);

            _context.SaveChanges();
        }

    }
}
