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
        /// Repositorio que se usa para leer y guardar categorias en la base de datos.
        /// </summary>
        private CategoriaRepo repo = new CategoriaRepo();

        /// <summary>
        /// Devuelve la lista de todas las categorias.
        /// </summary>
        /// <returns>Lista con todas las categorias de material.</returns>
        public List<CategoriaMaterial> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca categorias por un texto.
        /// Si el texto esta vacio, devuelve todas las categorias.
        /// </summary>
        /// <param name="texto">Texto para buscar por nombre o nivel.</param>
        /// <returns>Lista de categorias que coinciden con la busqueda.</returns>
        public List<CategoriaMaterial> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        /// <summary>
        /// Busca una categoria por su id.
        /// </summary>
        /// <param name="id">Id de la categoria a buscar.</param>
        /// <returns>La categoria encontrada o null si no existe.</returns>
        public CategoriaMaterial BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Crea una categoria nueva con los datos indicados.
        /// Si hay un error, devuelve un mensaje.
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

            CategoriaMaterial c = new CategoriaMaterial
            {
                NombreCategoria = nombreCategoria.Trim(),
                Nivel = nivel.Trim()
            };

            repo.Anyadir(c);
            return null;
        }

        /// <summary>
        /// Edita una categoria existente con los datos indicados.
        /// Si hay un error, devuelve un mensaje.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria a editar.</param>
        /// <param name="nombreCategoria">Nuevo nombre de la categoria.</param>
        /// <param name="nivel">Nuevo nivel de la categoria.</param>
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

            CategoriaMaterial c = new CategoriaMaterial
            {
                IdCategoria = idCategoria,
                NombreCategoria = nombreCategoria.Trim(),
                Nivel = nivel.Trim()
            };

            repo.Editar(c);
            return null;
        }

        /// <summary>
        /// Elimina una categoria si se puede borrar.
        /// Si tiene materiales relacionados, no se elimina.
        /// </summary>
        /// <param name="idCategoria">Id de la categoria a eliminar.</param>
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
