using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Api para gestionar materiales desde la interfaz.
    /// Valida los datos y llama al repositorio para guardar cambios.
    /// </summary>
    public class MaterialAPI
    {
        /// <summary>
        /// Repositorio usado para leer y guardar materiales.
        /// </summary>
        private MaterialRepo repo = new MaterialRepo();

        /// <summary>
        /// Devuelve la lista completa de materiales.
        /// </summary>
        /// <returns>Lista de materiales.</returns>
        public List<Material> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca un material por su id.
        /// </summary>
        /// <param name="id">Id del material.</param>
        /// <returns>Material si existe, o null si no existe.</returns>
        public Material BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Busca materiales por codigo.
        /// Puede devolver varios o ninguno.
        /// </summary>
        /// <param name="codigo">Texto a buscar dentro del codigo.</param>
        /// <returns>Lista de materiales que coinciden.</returns>
        public List<Material> BuscarPorCodigo(string codigo)
        {
            return repo.BuscarPorCodigo(codigo);
        }

        /// <summary>
        /// Crea un material nuevo despues de validar los datos.
        /// Devuelve null si se creo bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="codigo">Codigo del material.</param>
        /// <param name="marca">Marca del material.</param>
        /// <param name="modelo">Modelo del material.</param>
        /// <param name="talla">Talla o longitud.</param>
        /// <param name="estado">Estado del material.</param>
        /// <param name="precioDia">Precio por dia.</param>
        /// <param name="stock">Stock inicial.</param>
        /// <param name="idCategoria">Id de la categoria.</param>
        /// <returns>Null si se creo bien, o un mensaje si hubo error.</returns>
        public string Crear(string codigo, string marca, string modelo, string talla, string estado, decimal precioDia, int stock, int idCategoria)
        {
            string error = Validaciones.ValidarMaterial(codigo, marca, modelo, talla, estado, precioDia, stock, idCategoria);
            if (error != null)
            {
                return error;
            }

            var repetidos = repo.BuscarPorCodigo(codigo.Trim().ToUpper());
            if (repetidos.Count > 0)
            {
                return "Ya existe un material con ese codigo.";
            }


            Material material = new Material
            {
                Codigo = codigo.Trim(),
                Marca = marca.Trim(),
                Modelo = modelo.Trim(),
                TallaLongitud = talla.Trim(),
                Estado = estado.Trim(),
                PrecioDia = precioDia,
                Stock = stock,
                IdCategoria = idCategoria
            };

            repo.Anyadir(material);
            return null;
        }

        /// <summary>
        /// Edita un material existente despues de validar los datos.
        /// Devuelve null si se edito bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="id">Id del material.</param>
        /// <param name="codigo">Nuevo codigo.</param>
        /// <param name="marca">Nueva marca.</param>
        /// <param name="modelo">Nuevo modelo.</param>
        /// <param name="talla">Nueva talla o longitud.</param>
        /// <param name="estado">Nuevo estado.</param>
        /// <param name="precioDia">Nuevo precio por dia.</param>
        /// <param name="stock">Nuevo stock.</param>
        /// <param name="idCategoria">Nuevo id de categoria.</param>
        /// <returns>Null si se edito bien, o un mensaje si hubo error.</returns>
        public string Editar(int id, string codigo, string marca, string modelo, string talla, string estado, decimal precioDia, int stock, int idCategoria)
        {
            if (id <= 0)
            {
                return "Material no valido.";
            }

            string error = Validaciones.ValidarMaterial(codigo, marca, modelo, talla, estado, precioDia, stock, idCategoria);
            if (error != null)
            {
                return error;
            }

            var repetidos = repo.BuscarPorCodigo(codigo.Trim().ToUpper());
            foreach (var m in repetidos)
            {
                if (m.IdMaterial != id)
                {
                    return "Ya existe un material con ese codigo.";
                }
            }


            Material material = new Material
            {
                IdMaterial = id,
                Codigo = codigo.Trim(),
                Marca = marca.Trim(),
                Modelo = modelo.Trim(),
                TallaLongitud = talla.Trim(),
                Estado = estado.Trim(),
                PrecioDia = precioDia,
                Stock = stock,
                IdCategoria = idCategoria
            };

            repo.Editar(material);
            return null;
        }

        /// <summary>
        /// Elimina un material si se puede.
        /// Si esta en alguna linea de alquiler, no se elimina.
        /// </summary>
        /// <param name="id">Id del material.</param>
        /// <returns>Null si se elimino bien, o un mensaje si no se pudo.</returns>
        public string Eliminar(int id)
        {
            if (id <= 0)
            {
                return "Material no valido.";
            }

            if (repo.TieneLineasAlquiler(id))
            {
                return "No se puede eliminar el material porque existe en algun alquiler.";
            }

            bool eliminado = repo.Eliminar(id);
            if (!eliminado)
            {
                return "Error al eliminar material.";
            }

            return null;
        }

        /// <summary>
        /// Devuelve la cantidad total de materiales.
        /// </summary>
        /// <returns>Numero total de materiales.</returns>
        public int Cantidad()
        {
            return repo.Cantidad();
        }
    }
}
