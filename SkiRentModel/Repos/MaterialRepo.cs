using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para gestionar materiales en la base de datos.
    /// Permite listar, buscar, anadir, editar y eliminar.
    /// </summary>
    public class MaterialRepo
    {
        /// <summary>
        /// Contexto de base de datos usado para leer y guardar materiales.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve todos los materiales.
        /// </summary>
        /// <returns>Lista con todos los materiales.</returns>
        public List<Material> Listar()
        {
            return _context.Material.ToList();
        }

        /// <summary>
        /// Busca un material por su id.
        /// </summary>
        /// <param name="idMaterial">Id del material.</param>
        /// <returns>Material si existe, o null si no existe.</returns>
        public Material BuscarPorId(int idMaterial)
        {
            return _context.Material.Find(idMaterial);
        }

        /// <summary>
        /// Busca materiales cuyo codigo contiene el texto dado.
        /// Si el texto esta vacio devuelve todos.
        /// </summary>
        /// <param name="texto">Texto a buscar dentro del codigo.</param>
        /// <returns>Lista de materiales que coinciden.</returns>
        public List<Material> BuscarPorCodigo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return Listar();
            }

            texto = texto.Trim();

            return _context.Material
                .Where(m => m.Codigo.Contains(texto))
                .ToList();
        }

        /// <summary>
        /// Guarda un material nuevo en la base de datos.
        /// </summary>
        /// <param name="material">Material a guardar.</param>
        public void Anyadir(Material material)
        {
            _context.Material.Add(material);
            _context.SaveChanges();
        }

        /// <summary>
        /// Edita un material existente.
        /// Si no existe, no hace nada.
        /// </summary>
        /// <param name="materialActualizado">Material con los datos nuevos y el id correcto.</param>
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
        /// Elimina un material por su id si se puede.
        /// Si esta en lineas de alquiler, no se elimina.
        /// </summary>
        /// <param name="idMaterial">Id del material.</param>
        /// <returns>True si se elimino, false si no se pudo.</returns>
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

        /// <summary>
        /// Dice si un material esta en alguna linea de alquiler.
        /// </summary>
        /// <param name="idMaterial">Id del material.</param>
        /// <returns>True si existe una linea con ese material, false si no.</returns>
        public bool TieneLineasAlquiler(int idMaterial)
        {
            return _context.LineaAlquiler.Any(l => l.IdMaterial == idMaterial);
        }

        /// <summary>
        /// Devuelve la cantidad total de materiales.
        /// </summary>
        /// <returns>Numero total de materiales.</returns>
        public int Cantidad()
        {
            return _context.Material.Count();
        }
    }
}
