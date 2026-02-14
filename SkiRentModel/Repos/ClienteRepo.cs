using System;
using System.Collections.Generic;
using System.Linq;

namespace SkiRentModel.Repos
{
    public class ClienteRepo
    {
        private readonly SkiRentEntities _context = new SkiRentEntities();


        //Listar todos
        public List<Cliente> Listar()
        {
            return _context.Cliente.ToList();
        }

        //buscar por Id
        public Cliente BuscarPorId(int idCliente)
        {
            return _context.Cliente.Find(idCliente);
        }

        //buscar por dni 
        public List<Cliente> BuscarPorDni(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Listar();

            texto = texto.Trim();

            return _context.Cliente.Where(c => c.DNI.Contains(texto)).ToList();
        }

        //añadir
        public void Anyadir(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            _context.SaveChanges();
        }

        //editar
        public void Editar(Cliente clienteActualizado)
        {
            var clienteBD = _context.Cliente.Find(clienteActualizado.IdCliente);
            if (clienteBD == null) return;

            clienteBD.Nombre = clienteActualizado.Nombre;
            clienteBD.Apellidos = clienteActualizado.Apellidos;
            clienteBD.Telefono = clienteActualizado.Telefono;
            clienteBD.Email = clienteActualizado.Email;
            clienteBD.DNI = clienteActualizado.DNI;

            _context.SaveChanges();
        }

        //eliminar (devuelve false si no existe)
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

        public bool TieneAlquileres(int idCliente)
        {
            return _context.Alquiler.Any(a => a.IdCliente == idCliente);
        }




        public int Cantidad()
        {
            return _context.Cliente.Count();
        }


    }
}
