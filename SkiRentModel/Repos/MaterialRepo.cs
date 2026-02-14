using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    public class MaterialRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();



        public List<Material> Listar()
        {
            return _context.Material.ToList();
        }

        public Material BuscarPorId(int idMaterial)
        {
            return _context.Material.Find(idMaterial);
        }

        //buscar por codigo
        public List<Material> BuscarPorCodigo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return Listar();
            }
                

            texto = texto.Trim();

            return _context.Material.Where(m => m.Codigo.Contains(texto)).ToList();
        }

        public void Anyadir(Material material)
        {
            _context.Material.Add(material);
            _context.SaveChanges();
        }

        public void Editar(Material materialActualizado)
        {
            var materialBD = _context.Material.Find(materialActualizado.IdMaterial);
            if (materialBD == null)
            {
                return;
            }

            materialBD.Codigo = materialActualizado.Codigo;
            materialBD.Marca = materialActualizado.Marca;
            materialBD.Modelo = materialActualizado.Modelo;
            materialBD.TallaLongitud = materialActualizado.TallaLongitud;
            materialBD.Estado = materialActualizado.Estado;
            materialBD.PrecioDia = materialActualizado.PrecioDia;
            materialBD.Stock = materialActualizado.Stock;
            materialBD.IdCategoria = materialActualizado.IdCategoria;

            _context.SaveChanges();
        }

        public bool Eliminar(int idMaterial)
        {
            var materialBD = _context.Material.Find(idMaterial);
            if (materialBD == null)
            {
                return false;
            }

            if (TieneLineasAlquiler(idMaterial))
            {
                return false;
            }

            _context.Material.Remove(materialBD);
            _context.SaveChanges();
            return true;
        }

        //para que no se pueda borrar un material si se esta usando en una linea de alquiler
        public bool TieneLineasAlquiler(int idMaterial)
        {
            return _context.LineaAlquiler.Any(l => l.IdMaterial == idMaterial);
        }

        public int Cantidad()
        {
            return _context.Material.Count();
        }

    }
}
