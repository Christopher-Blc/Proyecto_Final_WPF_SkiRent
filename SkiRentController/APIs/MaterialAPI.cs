using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// API para gestionar materiales.
    /// Valida datos y usa el repositorio para operaciones.
    /// </summary>
    public class MaterialAPI
    {
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
        /// <param name="id">Id del material a buscar.</param>
        /// <returns>El material si existe, o null si no se encuentra.</returns>
        public Material BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Busca materiales por codigo, puede devolver muchos o ninguno.
        /// </summary>
        /// <param name="codigo">Texto a buscar dentro del codigo.</param>
        /// <returns>Lista de materiales que coinciden.</returns>
        public List<Material> BuscarPorCodigo(string codigo)
        {
            return repo.BuscarPorCodigo(codigo);
        }

        /// <summary>
        /// Crea un nuevo material despues de validar los datos.
        /// Devuelve null si todo va bien o un mensaje de error si falla.
        /// </summary>
        /// <param name="codigo">Codigo del material.</param>
        /// <param name="marca">Marca del material.</param>
        /// <param name="modelo">Modelo del material.</param>
        /// <param name="talla">Talla o longitud.</param>
        /// <param name="estado">Estado del material.</param>
        /// <param name="precioDia">Precio por dia.</param>
        /// <param name="stock">Cantidad en stock.</param>
        /// <param name="idCategoria">Id de la categoria.</param>
        /// <returns>Null si se crea bien, o texto con el error.</returns>
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
                //trim y tu upper solo es para que si hay un espacio o una mayscula en vez de minuscula , evitar que nos locepte
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

        /// <summary>
        /// Edita un material ya existente.
        /// Devuelve null si todo va bien o un mensaje de error.
        /// </summary>
        /// <param name="id">Id del material a editar.</param>
        /// <param name="codigo">Nuevo codigo.</param>
        /// <param name="marca">Nueva marca.</param>
        /// <param name="modelo">Nuevo modelo.</param>
        /// <param name="talla">Nueva talla o longitud.</param>
        /// <param name="estado">Nuevo estado.</param>
        /// <param name="precioDia">Nuevo precio por dia.</param>
        /// <param name="stock">Nuevo stock.</param>
        /// <param name="idCategoria">Nuevo id de categoria.</param>
        /// <returns>Null si se edita bien, o texto con el error.</returns>
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
        /// Elimina un material si no esta en uso.
        /// Devuelve null si se borra, o un mensaje de error.
        /// </summary>
        /// <param name="id">Id del material a eliminar.</param>
        /// <returns>Null si se elimina, o texto con el error.</returns>
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

/* Plantilla de comentarios XML para los repositorios (copiar en los archivos de repo correspondientes)
   Nota: texto simple en español sin acentos ni tecnicismos.
   
   public class MaterialRepo {
       /// <summary>
       /// Contexto de la base de datos usado para las operaciones.
       /// </summary>
       private readonly SkiRentEntities _context = new SkiRentEntities();

       /// <summary>
       /// Devuelve todos los materiales guardados.
       /// </summary>
       /// <returns>Lista con todos los materiales.</returns>
       public List<Material> Listar();

       /// <summary>
       /// Busca un material por su id.
       /// </summary>
       /// <param name="idMaterial">Id del material a buscar.</param>
       /// <returns>El material si existe, o null si no se encuentra.</returns>
       public Material BuscarPorId(int idMaterial);

       /// <summary>
       /// Busca materiales cuyo codigo contenga el texto dado.
       /// Si el texto esta vacio o solo espacios devuelve todos los materiales.
       /// </summary>
       /// <param name="texto">Texto que debe contener el codigo.</param>
       /// <returns>Lista de materiales que cumplen la condicion.</returns>
       public List<Material> BuscarPorCodigo(string texto);

       /// <summary>
       /// Anyade un nuevo material a la base de datos.
       /// </summary>
       /// <param name="material">Objeto material a guardar.</param>
       public void Anyadir(Material material);

       /// <summary>
       /// Actualiza los datos de un material existente.
       /// Si no se encuentra el material no hace nada.
       /// </summary>
       /// <param name="materialActualizado">Objeto con los datos actualizados. Debe incluir el IdMaterial.</param>
       public void Editar(Material materialActualizado);

       /// <summary>
       /// Elimina un material por su id si no esta siendo usado en lineas de alquiler.
       /// </summary>
       /// <param name="idMaterial">Id del material a eliminar.</param>
       /// <returns>True si se elimino, false si no se encontro o si esta en uso.</returns>
       public bool Eliminar(int idMaterial);

       /// <summary>
       /// Comprueba si un material tiene lineas de alquiler asociadas.
       /// </summary>
       /// <param name="idMaterial">Id del material a comprobar.</param>
       /// <returns>True si existe alguna linea de alquiler con el material, false si no.</returns>
       public bool TieneLineasAlquiler(int idMaterial);

       /// <summary>
       /// Cuenta cuantos materiales hay en la base de datos.
       /// </summary>
       /// <returns>Numero total de materiales.</returns>
       public int Cantidad();
   }
*/
