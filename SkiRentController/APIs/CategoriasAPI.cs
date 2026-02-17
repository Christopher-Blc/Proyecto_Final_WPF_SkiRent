using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Api para gestionar las categorias de material.
    /// Contiene metodos para listar, buscar, crear, editar y eliminar categorias.
    /// </summary>
    public class CategoriaAPI
    {
        /// <summary>
        /// Repositorio usado para leer y guardar categorias.
        /// </summary>
        private CategoriaRepo repo = new CategoriaRepo();

        /// <summary>
        /// Devuelve la lista de todas las categorias.
        /// </summary>
        /// <returns>Lista con todas las categorias.</returns>
        public List<CategoriaMaterial> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca categorias por un texto.
        /// Si el texto esta vacio, devuelve todas.
        /// </summary>
        /// <param name="texto">Texto para buscar por nombre o nivel.</param>
        /// <returns>Lista de categorias que coinciden.</returns>
        public List<CategoriaMaterial> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        /// <summary>
        /// Busca una categoria por su id.
        /// </summary>
        /// <param name="id">Id de la categoria.</param>
        /// <returns>Categoria si existe, o null si no existe.</returns>
        public CategoriaMaterial BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Crea una categoria nueva.
        /// Devuelve null si fue bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="nombreCategoria">Nombre de la categoria.</param>
        /// <param name="nivel">Nivel de la categoria.</param>
        /// <returns>Null si se creo bien, o un mensaje si hubo error.</returns>
        public string Crear(string nombreCategoria, string nivel)
        {
            string error = Validaciones.ValidarCategoria(nombreCategoria, nivel);
            if (error != null)
            {
                return error;
            }

            string nombre = nombreCategoria.Trim();
            string niv = nivel.Trim();

            // Comprobar duplicado sencillo (mismo nombre y nivel)
            foreach (var cat in repo.Listar())
            {
                if (cat.NombreCategoria != null && cat.Nivel != null)
                {
                    if (cat.NombreCategoria.Trim().ToUpper() == nombre.ToUpper() &&
                        cat.Nivel.Trim().ToUpper() == niv.ToUpper())
                    {
                        return "Ya existe una categoria con ese nombre y nivel.";
                    }
                }
            }

            CategoriaMaterial c = new CategoriaMaterial
            {
                NombreCategoria = nombre,
                Nivel = niv
            };

            repo.Anyadir(c);
            return null;
        }

        /// <summary>
        /// Edita una categoria existente.
        /// Devuelve null si fue bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria.</param>
        /// <param name="nombreCategoria">Nuevo nombre.</param>
        /// <param name="nivel">Nuevo nivel.</param>
        /// <returns>Null si se edito bien, o un mensaje si hubo error.</returns>
        public string Editar(int idCategoria, string nombreCategoria, string nivel)
        {
            if (idCategoria <= 0)
            {
                return "Categoria no valida.";
            }

            string error = Validaciones.ValidarCategoria(nombreCategoria, nivel);
            if (error != null)
            {
                return error;
            }

            string nombre = nombreCategoria.Trim();
            string niv = nivel.Trim();

            // Comprobar duplicado excluyendo su propio id
            foreach (var cat in repo.Listar())
            {
                if (cat.IdCategoria != idCategoria && cat.NombreCategoria != null && cat.Nivel != null)
                {
                    if (cat.NombreCategoria.Trim().ToUpper() == nombre.ToUpper() &&
                        cat.Nivel.Trim().ToUpper() == niv.ToUpper())
                    {
                        return "Ya existe una categoria con ese nombre y nivel.";
                    }
                }
            }

            CategoriaMaterial c = new CategoriaMaterial
            {
                IdCategoria = idCategoria,
                NombreCategoria = nombre,
                Nivel = niv
            };

            repo.Editar(c);
            return null;
        }

        /// <summary>
        /// Elimina una categoria si se puede borrar.
        /// Si tiene materiales relacionados, no se elimina.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria.</param>
        /// <returns>Null si se elimino bien, o un mensaje si no se pudo.</returns>
        public string Eliminar(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                return "Categoria no valida.";
            }

            if (repo.TieneMateriales(idCategoria))
            {
                return "No se puede eliminar la categoria porque tiene materiales.";
            }

            bool eliminado = repo.Eliminar(idCategoria);
            if (!eliminado)
            {
                return "Error al eliminar categoria.";
            }

            return null;
        }
    }
}
