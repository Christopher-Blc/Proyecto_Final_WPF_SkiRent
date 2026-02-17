using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    /// <summary>
    /// Repositorio para trabajar con clientes en la base de datos.
    /// Permite listar, buscar, anadir, editar y eliminar.
    /// </summary>
    public class ClienteRepo
    {
        /// <summary>
        /// Contexto de base de datos usado para leer y guardar clientes.
        /// </summary>
        private readonly SkiRentEntities _context = new SkiRentEntities();

        /// <summary>
        /// Devuelve todos los clientes.
        /// </summary>
        /// <returns>Lista con todos los clientes.</returns>
        public List<Cliente> Listar()
        {
            return _context.Cliente.ToList();
        }

        /// <summary>
        /// Busca un cliente por su id.
        /// </summary>
        /// <param name="idCliente">Id del cliente a buscar.</param>
        /// <returns>Cliente si existe, o null si no existe.</returns>
        public Cliente BuscarPorId(int idCliente)
        {
            return _context.Cliente.Find(idCliente);
        }

        /// <summary>
        /// Busca clientes cuyo DNI contiene el texto dado.
        /// Si el texto esta vacio devuelve todos.
        /// </summary>
        /// <param name="texto">Texto a buscar dentro del DNI.</param>
        /// <returns>Lista de clientes que coinciden con la busqueda.</returns>
        public List<Cliente> BuscarPorDni(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return Listar();
            }

            texto = texto.Trim();

            return _context.Cliente
                .Where(c => c.DNI.Contains(texto))
                .ToList();
        }

        /// <summary>
        /// Anade un cliente nuevo a la base de datos.
        /// </summary>
        /// <param name="cliente">Cliente que se quiere guardar.</param>
        public void Anyadir(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            _context.SaveChanges();
        }

        /// <summary>
        /// Edita los datos de un cliente existente.
        /// Si no existe, no hace nada.
        /// </summary>
        /// <param name="clienteActualizado">Cliente con los datos nuevos y el id correcto.</param>
        public void Editar(Cliente clienteActualizado)
        {
            var clienteBD = _context.Cliente.Find(clienteActualizado.IdCliente);
            if (clienteBD == null)
            {
                return;
            }

            clienteBD.Nombre = clienteActualizado.Nombre;
            clienteBD.Apellidos = clienteActualizado.Apellidos;
            clienteBD.Telefono = clienteActualizado.Telefono;
            clienteBD.Email = clienteActualizado.Email;
            clienteBD.DNI = clienteActualizado.DNI;

            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina un cliente si existe y no tiene alquileres.
        /// </summary>
        /// <param name="idCliente">Id del cliente a eliminar.</param>
        /// <returns>True si se elimino, false si no se pudo.</returns>
        public bool Eliminar(int idCliente)
        {
            var clienteBD = _context.Cliente.Find(idCliente);
            if (clienteBD == null)
            {
                return false;
            }

            if (TieneAlquileres(idCliente))
            {
                return false;
            }

            _context.Cliente.Remove(clienteBD);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Dice si un cliente tiene alquileres guardados.
        /// </summary>
        /// <param name="idCliente">Id del cliente a comprobar.</param>
        /// <returns>True si tiene alquileres, false si no.</returns>
        public bool TieneAlquileres(int idCliente)
        {
            return _context.Alquiler.Any(a => a.IdCliente == idCliente);
        }

        /// <summary>
        /// Devuelve la cantidad total de clientes.
        /// </summary>
        /// <returns>Numero total de clientes.</returns>
        public int Cantidad()
        {
            return _context.Cliente.Count();
        }
    }
}
