using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    public class AlquilerRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();

        public List<Alquiler> Listar()
        {
            return _context.Alquiler.ToList();
        }

        public Alquiler BuscarPorId(int idAlquiler)
        {
            return _context.Alquiler.Find(idAlquiler);
        }

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

        public void Anyadir(Alquiler alquiler)
        {
            _context.Alquiler.Add(alquiler);
            _context.SaveChanges();
        }

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

        public int CantidadActivos()
        {
            return _context.Alquiler.Count(a => a.Estado == "Abierto");
        }

        public bool TieneLineas(int idAlquiler)
        {
            return _context.LineaAlquiler.Any(l => l.IdAlquiler == idAlquiler);
        }
    }
}
