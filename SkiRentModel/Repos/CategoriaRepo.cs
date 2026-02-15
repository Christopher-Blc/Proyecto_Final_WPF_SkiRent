using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para manejar las operaciones con las categorias de material.
    /// </summary>
    public class CategoriaRepo
    {
        /// <summary>
        /// Contexto de la base de datos para acceder a las tablas.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve todas las categorias.
        /// </summary>
        /// <returns>Lista con todas las categorias de material.</returns>
        public List<CategoriaMaterial> Listar()
        {
            return _context.CategoriaMaterial.ToList();
        }

        /// <summary>
        /// Busca una categoria por su id.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria a buscar.</param>
        /// <returns>La categoria encontrada o null si no existe.</returns>
        public CategoriaMaterial BuscarPorId(int idCategoria)
        {
            return _context.CategoriaMaterial.Find(idCategoria);
        }

        /// <summary>
        /// Busca categorias por nombre o por nivel.
        /// Si el texto esta vacio devuelve todas las categorias.
        /// </summary>
        /// <param name="texto">Texto para buscar en nombre o nivel.</param>
        /// <returns>Lista de categorias que coinciden con la busqueda.</returns>
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

        /// <summary>
        /// Anyade una nueva categoria a la base de datos.
        /// </summary>
        /// <param name="categoria">Objeto categoria a anyadir.</param>
        public void Anyadir(CategoriaMaterial categoria)
        {
            _context.CategoriaMaterial.Add(categoria);
            _context.SaveChanges();
        }

        /// <summary>
        /// Actualiza los datos de una categoria existente.
        /// </summary>
        /// <param name="categoriaActualizada">Categoria con los datos actualizados (debe incluir Id).</param>
        public void Editar(CategoriaMaterial categoriaActualizada)
        {
            var categoriaBD = _context.CategoriaMaterial
                                      .Find(categoriaActualizada.IdCategoria);

            if (categoriaBD == null) return;

            categoriaBD.NombreCategoria = categoriaActualizada.NombreCategoria;
            categoriaBD.Nivel = categoriaActualizada.Nivel;

            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina una categoria por su id.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria a eliminar.</param>
        /// <returns>True si se elimino, false si no se encontro.</returns>
        public bool Eliminar(int idCategoria)
        {
            var categoriaBD = _context.CategoriaMaterial.Find(idCategoria);
            if (categoriaBD == null) return false;

            _context.CategoriaMaterial.Remove(categoriaBD);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Comprueba si hay materiales relacionados con la categoria.
        /// Esto se usa para evitar borrar una categoria que tiene materiales.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria a comprobar.</param>
        /// <returns>True si existe al menos un material con esa categoria, false si no.</returns>
        public bool TieneMateriales(int idCategoria)
        {
            return _context.Material.Any(m => m.IdCategoria == idCategoria);
        }


    }
}
