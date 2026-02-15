using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para gestionar los materiales en la base de datos.
    /// Contiene operaciones basicas como listar, buscar, añadir, editar y borrar.
    /// </summary>
    public class MaterialRepo
    {
        /// <summary>
        /// Contexto de la base de datos usado para las operaciones.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();


                                                                            
        /// <summary>
        /// Devuelve todos los materiales guardados.
        /// </summary>
        /// <returns>Lista con todos los materiales.</returns>
        public List<Material> Listar()
        {
            return _context.Material.ToList();
        }

        /// <summary>
        /// Busca un material por su id.
        /// </summary>
        /// <param name="idMaterial">Identificador del material a buscar.</param>
        /// <returns>El material si existe, o null si no se encuentra.</returns>
        public Material BuscarPorId(int idMaterial)
        {
            return _context.Material.Find(idMaterial);
        }

        //buscar por codigo
        /// <summary>
        /// Busca materiales cuyo codigo contenga el texto dado.
        /// Si el texto esta vacio o solo espacios devuelve todos los materiales.
        /// </summary>
        /// <param name="texto">Texto que debe contener el codigo.</param>
        /// <returns>Lista de materiales que cumplen la condicion.</returns>
        public List<Material> BuscarPorCodigo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return Listar();
            }
                

            texto = texto.Trim();

            return _context.Material.Where(m => m.Codigo.Contains(texto)).ToList();
        }

        /// <summary>
        /// Anyade un nuevo material a la base de datos.
        /// </summary>
        /// <param name="material">Objeto material a guardar.</param>
        public void Anyadir(Material material)
        {
            _context.Material.Add(material);
            _context.SaveChanges();
        }

        /// <summary>
        /// Actualiza los datos de un material existente.
        /// Si no se encuentra el material no hace nada.
        /// </summary>
        /// <param name="materialActualizado">Objeto con los datos actualizados. Debe incluir el IdMaterial.</param>
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

        /// <summary>
        /// Elimina un material por su id si no esta siendo usado en lineas de alquiler.
        /// </summary>
        /// <param name="idMaterial">Identificador del material a eliminar.</param>
        /// <returns>True si se elimino, false si no se encontro o si esta en uso.</returns>
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
        /// <summary>
        /// Comprueba si un material tiene lineas de alquiler asociadas.
        /// </summary>
        /// <param name="idMaterial">Identificador del material a comprobar.</param>
        /// <returns>True si existe alguna linea de alquiler con el material, false si no.</returns>
        public bool TieneLineasAlquiler(int idMaterial)
        {
            return _context.LineaAlquiler.Any(l => l.IdMaterial == idMaterial);
        }

        /// <summary>
        /// Cuenta cuantos materiales hay en la base de datos.
        /// </summary>
        /// <returns>Numero total de materiales.</returns>
        public int Cantidad()
        {
            return _context.Material.Count();
        }

    }
}
