using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    public class AlquilerRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();

        private bool EsEstadoFinal(string estado)
        {
            estado = (estado ?? "").Trim();
            return estado.Equals("Cancelado", StringComparison.OrdinalIgnoreCase)
                || estado.Equals("Cerrado", StringComparison.OrdinalIgnoreCase);
        }

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
            texto = (texto ?? "").Trim();
            if (texto == "")
            {
                return Listar();
            }

            bool esNumero = int.TryParse(texto, out int idCliente);

            return _context.Alquiler
                .Where(a => a.Estado.Contains(texto) || (esNumero && a.IdCliente == idCliente))
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
            if (alquilerBD == null)
            {
                return;
            }

            string estadoAnterior = alquilerBD.Estado ?? "";
            string nuevoEstado = alquilerActualizado.Estado ?? "";

            if (!estadoAnterior.Equals(nuevoEstado, StringComparison.OrdinalIgnoreCase) && EsEstadoFinal(nuevoEstado))
            {
                DevolverStock(alquilerBD.IdAlquiler);
            }

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

            var lineas = _context.LineaAlquiler.Where(l => l.IdAlquiler == idAlquiler).ToList();
            bool estadoFinal = EsEstadoFinal(alquilerBD.Estado);

            if (!estadoFinal && lineas.Count > 0)
            {
                DevolverStockConLineas(lineas);
            }

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

        private void DevolverStock(int idAlquiler)
        {
            var lineas = _context.LineaAlquiler.Where(l => l.IdAlquiler == idAlquiler).ToList();
            DevolverStockConLineas(lineas);
        }

        private void DevolverStockConLineas(List<LineaAlquiler> lineas)
        {
            if (lineas == null || lineas.Count == 0)
            {
                return;
            }

            foreach (var linea in lineas)
            {
                var material = _context.Material.Find(linea.IdMaterial);
                if (material != null)
                {
                    material.Stock += linea.Cantidad;
                }
            }
        }
    }
}
