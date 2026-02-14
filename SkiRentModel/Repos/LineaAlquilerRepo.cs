using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    public class LineaAlquilerRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();

        public List<LineaAlquiler> ListarPorAlquiler(int idAlquiler)
        {
            return _context.LineaAlquiler
                           .Where(l => l.IdAlquiler == idAlquiler)
                           .ToList();
        }

        //añadir linea solo si el alquiler esta Abierto(stock se reserva)
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

        // Eliminar la linea , solo si el alquiler esta en Abierto(stock se devuelve)
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

        // Recalcula total del alquiler
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
