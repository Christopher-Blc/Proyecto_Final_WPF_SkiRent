using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    public class MaterialAPI
    {
        private MaterialRepo repo = new MaterialRepo();

        public List<Material> Listar()
        {
            return repo.Listar();
        }

        public Material BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        public List<Material> BuscarPorCodigo(string codigo)
        {
            return repo.BuscarPorCodigo(codigo);
        }

        public string Crear(string codigo, string marca, string modelo, string talla, string estado,decimal precioDia, int stock, int idCategoria)
        {
            //validamos
            string error = Validaciones.ValidarMaterial(codigo, marca, modelo, talla, estado, precioDia, stock, idCategoria);

            if (error != null)
            {
                return error;
            }

            //comprobar que no se este utilizando ya el codigo del producto
            foreach (var m in repo.Listar())
            {
                //trim y tu upper solo es para que si hay un espacio o una mayscula en vez de minuscula , evitar que nos lo accepte
                if (m.Codigo != null && m.Codigo.Trim().ToUpper() == codigo.Trim().ToUpper())
                {
                    return "Ya existe un material con ese codigo.";
                }
            }

            //creamos el material
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

            //si no falla nada , lo añadimos
            repo.Anyadir(material);
            return null;
        }

        public string Editar(int id, string codigo, string marca, string modelo, string talla, string estado,decimal precioDia, int stock, int idCategoria)
        {
            //mirar que el id sea valido
            if (id <= 0)
            {
                return "Material no valido.";
            }

            //coger el error y pasarlo a la view por si falla algo al validar
            string error = Validaciones.ValidarMaterial( codigo, marca, modelo, talla, estado, precioDia, stock, idCategoria);

            if (error != null)
            {
                return error;
            }

            //comprobamos duplicados tambien aqui pero teniendo en cuenta su propio id por si no se cambia y se edita solo otra cosa
            foreach (var m in repo.Listar())
            {
                if (m.IdMaterial != id && m.Codigo != null && m.Codigo.Trim().ToUpper() == codigo.Trim().ToUpper())
                {
                    return "Ya existe un material con ese código.";
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

        public int Cantidad()
        {
            return repo.Cantidad();
        }
    }
}
