using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    public class CategoriaRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();

        // Listar todas
        public List<CategoriaMaterial> Listar()
        {
            return _context.CategoriaMaterial.ToList();
        }

        // Buscar por Id
        public CategoriaMaterial BuscarPorId(int idCategoria)
        {
            return _context.CategoriaMaterial.Find(idCategoria);
        }

        // Buscar por nombre o nivel
        public List<CategoriaMaterial> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Listar();

            texto = texto.Trim();

            return _context.CategoriaMaterial
                .Where(c =>
                    c.NombreCategoria.Contains(texto) ||
                    c.Nivel.Contains(texto)
                )
                .ToList();
        }

        // Añadir
        public void Anyadir(CategoriaMaterial categoria)
        {
            _context.CategoriaMaterial.Add(categoria);
            _context.SaveChanges();
        }

        // Editar
        public void Editar(CategoriaMaterial categoriaActualizada)
        {
            var categoriaBD = _context.CategoriaMaterial
                                      .Find(categoriaActualizada.IdCategoria);

            if (categoriaBD == null) return;

            categoriaBD.NombreCategoria = categoriaActualizada.NombreCategoria;
            categoriaBD.Nivel = categoriaActualizada.Nivel;

            _context.SaveChanges();
        }

        // Eliminar
        public bool Eliminar(int idCategoria)
        {
            var categoriaBD = _context.CategoriaMaterial.Find(idCategoria);
            if (categoriaBD == null) return false;

            _context.CategoriaMaterial.Remove(categoriaBD);
            _context.SaveChanges();
            return true;
        }

        //para no borrar si ya hay materiales con esa categoria
        public bool TieneMateriales(int idCategoria)
        {
            return _context.Material.Any(m => m.IdCategoria == idCategoria);
        }


    }
}
