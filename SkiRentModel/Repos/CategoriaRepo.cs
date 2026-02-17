using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para manejar las categorias de material en la base de datos.
    /// Permite listar, buscar, anadir, editar y eliminar categorias.
    /// </summary>
    public class CategoriaRepo
    {
        /// <summary>
        /// Contexto de base de datos usado para trabajar con las categorias.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve todas las categorias guardadas.
        /// </summary>
        /// <returns>Lista con todas las categorias.</returns>
        public List<CategoriaMaterial> Listar()
        {
            return _context.CategoriaMaterial.ToList();
        }

        /// <summary>
        /// Busca categorias por texto en nombre o nivel.
        /// Si el texto esta vacio devuelve todas.
        /// </summary>
        /// <param name="texto">Texto para buscar.</param>
        /// <returns>Lista de categorias que coinciden.</returns>
        public List<CategoriaMaterial> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return Listar();
            }

            texto = texto.Trim();

            return _context.CategoriaMaterial
                .Where(c =>
                    c.NombreCategoria.Contains(texto) ||
                    c.Nivel.Contains(texto)
                )
                .ToList();
        }

        /// <summary>
        /// Busca una categoria por su id.
        /// </summary>
        /// <param name="id">Id de la categoria.</param>
        /// <returns>Categoria encontrada o null.</returns>
        public CategoriaMaterial BuscarPorId(int id)
        {
            return _context.CategoriaMaterial.Find(id);
        }

        /// <summary>
        /// Guarda una categoria nueva en la base de datos.
        /// </summary>
        /// <param name="categoria">Categoria a guardar.</param>
        public void Anyadir(CategoriaMaterial categoria)
        {
            _context.CategoriaMaterial.Add(categoria);
            _context.SaveChanges();
        }

        /// <summary>
        /// Edita una categoria existente.
        /// Si no existe, no hace nada.
        /// </summary>
        /// <param name="categoriaActualizada">Categoria con los datos nuevos.</param>
        public void Editar(CategoriaMaterial categoriaActualizada)
        {
            var categoriaBD = _context.CategoriaMaterial.Find(categoriaActualizada.IdCategoria);
            if (categoriaBD == null)
            {
                return;
            }

            categoriaBD.NombreCategoria = categoriaActualizada.NombreCategoria;
            categoriaBD.Nivel = categoriaActualizada.Nivel;

            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina una categoria por su id.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria a eliminar.</param>
        /// <returns>True si se elimino, false si no existe.</returns>
        public bool Eliminar(int idCategoria)
        {
            var categoriaBD = _context.CategoriaMaterial.Find(idCategoria);
            if (categoriaBD == null)
            {
                return false;
            }

            _context.CategoriaMaterial.Remove(categoriaBD);
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Comprueba si una categoria tiene materiales asociados.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria.</param>
        /// <returns>True si tiene materiales, false si no.</returns>
        public bool TieneMateriales(int idCategoria)
        {
            return _context.Material.Any(m => m.IdCategoria == idCategoria);
        }
    }
}
