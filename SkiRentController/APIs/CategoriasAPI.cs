using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    public class CategoriaAPI
    {
        private CategoriaRepo repo = new CategoriaRepo();

        public List<CategoriaMaterial> Listar()
        {
            return repo.Listar();
        }

        public List<CategoriaMaterial> Buscar(string texto)
        {
            return repo.Buscar(texto);
        }

        public CategoriaMaterial BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

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
