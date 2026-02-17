using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// Api para gestionar clientes desde la interfaz.
    /// Permite listar, buscar, crear, editar y eliminar clientes.
    /// </summary>
    public class ClienteAPI
    {
        /// <summary>
        /// Repositorio usado para leer y guardar clientes.
        /// </summary>
        private ClienteRepo repo = new ClienteRepo();

        /// <summary>
        /// Devuelve todos los clientes.
        /// </summary>
        /// <returns>Lista con todos los clientes.</returns>
        public List<Cliente> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca clientes cuyo DNI contiene el texto dado.
        /// Si el texto esta vacio devuelve todos.
        /// </summary>
        /// <param name="texto">Texto a buscar dentro del DNI.</param>
        /// <returns>Lista de clientes que coinciden con la busqueda.</returns>
        public List<Cliente> Buscar(string texto)
        {
            return repo.BuscarPorDni(texto);
        }

        /// <summary>
        /// Busca un cliente por su id.
        /// </summary>
        /// <param name="id">Id del cliente a buscar.</param>
        /// <returns>Cliente si existe, o null si no existe.</returns>
        public Cliente BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Crea un cliente nuevo despues de validar los datos.
        /// Devuelve null si se creo bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="nombre">Nombre del cliente.</param>
        /// <param name="apellidos">Apellidos del cliente.</param>
        /// <param name="telefono">Telefono del cliente.</param>
        /// <param name="email">Email del cliente.</param>
        /// <param name="dni">DNI del cliente.</param>
        /// <returns>Null si se creo bien, o un mensaje si hubo error.</returns>
        public string Crear(string nombre, string apellidos, string telefono, string email, string dni)
        {
            string error = Validaciones.ValidarCliente(nombre, apellidos, dni, telefono, email);
            if (error != null)
            {
                return error;
            }

            // comprobar duplicado usando una busqueda en BD
            var repetidos = repo.BuscarPorDni(dni.Trim().ToUpper());
            if (repetidos.Count > 0)
            {
                return "Ya existe un cliente con ese DNI.";
            }

            Cliente cliente = new Cliente
            {
                Nombre = nombre.Trim(),
                Apellidos = apellidos.Trim(),
                Telefono = telefono.Trim(),
                Email = email.Trim(),
                DNI = dni.Trim().ToUpper()
            };

            repo.Anyadir(cliente);
            return null;
        }


        /// <summary>
        /// Edita un cliente existente despues de validar los datos.
        /// Devuelve null si se edito bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="id">Id del cliente a editar.</param>
        /// <param name="nombre">Nombre nuevo del cliente.</param>
        /// <param name="apellidos">Apellidos nuevos del cliente.</param>
        /// <param name="telefono">Telefono nuevo del cliente.</param>
        /// <param name="email">Email nuevo del cliente.</param>
        /// <param name="dni">DNI nuevo del cliente.</param>
        /// <returns>Null si se edito bien, o un mensaje si hubo error.</returns>
        public string Editar(int id, string nombre, string apellidos, string telefono, string email, string dni)
        {
            if (id <= 0)
            {
                return "Cliente no valido.";
            }

            string error = Validaciones.ValidarCliente(nombre, apellidos, dni, telefono, email);
            if (error != null)
            {
                return error;
            }

            var repetidos = repo.BuscarPorDni(dni.Trim().ToUpper());
            foreach (var c in repetidos)
            {
                if (c.IdCliente != id)
                {
                    return "Ya existe un cliente con ese DNI.";
                }
            }

            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Nombre = nombre.Trim(),
                Apellidos = apellidos.Trim(),
                Telefono = telefono.Trim(),
                Email = email.Trim(),
                DNI = dni.Trim().ToUpper()
            };

            repo.Editar(cliente);
            return null;
        }


        /// <summary>
        /// Elimina un cliente por id.
        /// Devuelve null si se elimino bien, o un mensaje si hubo error.
        /// </summary>
        /// <param name="id">Id del cliente a eliminar.</param>
        /// <returns>Null si se elimino bien, o un mensaje si hubo error.</returns>
        public string Eliminar(int id)
        {
            if (id <= 0)
            {
                return "Cliente no valido.";
            }

            bool eliminadoCorrectamente = repo.Eliminar(id);
            if (!eliminadoCorrectamente)
            {
                return "Error al eliminar.";
            }

            return null;
        }

        /// <summary>
        /// Devuelve la cantidad total de clientes.
        /// </summary>
        /// <returns>Numero total de clientes.</returns>
        public int Cantidad()
        {
            return repo.Cantidad();
        }
    }
}
