using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Proyecto_WPF_SkiRent.Controllers
{
    /// <summary>
    /// API para manejar operaciones de cliente desde la capa de presentacion.
    /// Contiene metodos para listar, buscar, crear, editar y eliminar clientes.
    /// </summary>
    public class ClienteAPI
    {
        private ClienteRepo repo = new ClienteRepo();

        /// <summary>
        /// Devuelve todos los clientes.
        /// </summary>
        /// <returns>Lista con todos los clientes</returns>
        public List<Cliente> Listar()
        {
            return repo.Listar();
        }

        /// <summary>
        /// Busca clientes cuyo DNI contiene el texto dado.
        /// </summary>
        /// <param name="texto">Texto a buscar dentro del DNI</param>
        /// <returns>Lista de clientes que coinciden con la busqueda</returns>
        public List<Cliente> Buscar(string texto)
        {
            return repo.BuscarPorDni(texto);
        }

        /// <summary>
        /// Busca un cliente por su id.
        /// </summary>
        /// <param name="id">Id del cliente a buscar</param>
        /// <returns>Cliente si existe, sino null</returns>
        public Cliente BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        /// <summary>
        /// Crea un nuevo cliente despues de validar sus datos.
        /// Devuelve null si todo va bien, o un texto con el error.
        /// </summary>
        /// <param name="nombre">Nombre del cliente</param>
        /// <param name="apellidos">Apellidos del cliente</param>
        /// <param name="telefono">Telefono del cliente</param>
        /// <param name="email">Email del cliente</param>
        /// <param name="dni">DNI del cliente</param>
        /// <returns>Null si se creo bien, o mensaje de error</returns>
        public string Crear(string nombre, string apellidos, string telefono, string email, string dni)
        {
            //validamos con el validator de la clase validaciones que hemos hecho 
            //y si devuelve un error lo devolvemos y se mostrara , si no hay error , 
            //devuelve null
            string error = Validaciones.ValidarCliente(nombre, apellidos, dni, telefono, email);
            if (error != null)
            {
                return error;
            }
            foreach (var c in repo.Listar())
            {
                if (c.DNI != null && c.DNI.Trim().ToUpper() == dni.Trim().ToUpper())
                    return "Ya existe un cliente con ese DNI.";
            }


            //crear cliente
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
        /// Edita un cliente existente tras validar los datos.
        /// Devuelve null si todo va bien, o un texto con el error.
        /// </summary>
        /// <param name="id">Id del cliente a editar</param>
        /// <param name="nombre">Nombre nuevo</param>
        /// <param name="apellidos">Apellidos nuevos</param>
        /// <param name="telefono">Telefono nuevo</param>
        /// <param name="email">Email nuevo</param>
        /// <param name="dni">DNI nuevo</param>
        /// <returns>Null si se edito bien, o mensaje de error</returns>
        public string Editar(int id, string nombre, string apellidos, string telefono, string email, string dni)
        {
            if (id <= 0)
            {
                return "Cliente no valido.";
            }

            //se valida igual que hemos hecho en crear antes
            string error = Validaciones.ValidarCliente(nombre, apellidos, dni, telefono, email);
            if (error != null)
            {
                return error;
            }
            foreach (var c in repo.Listar())
            {
                if (c.IdCliente != id && c.DNI != null && c.DNI.Trim().ToUpper() == dni.Trim().ToUpper())
                    return "Ya existe un cliente con ese DNI.";
            }


            //editamos el cliente
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
        /// Elimina un cliente por id si es valido y no tiene alquileres.
        /// Devuelve null si se elimino bien, o un texto con el error.
        /// </summary>
        /// <param name="id">Id del cliente a eliminar</param>
        /// <returns>Null si se elimino bien, o mensaje de error</returns>
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

            //se devuelve null pq si no hay error no hay string de error a devolver
            return null;
        }
        /// <summary>
        /// Devuelve el numero total de clientes para mostrar en el dashboard.
        /// </summary>
        /// <returns>Cantidad total de clientes</returns>
        public int Cantidad()
        {
            return repo.Cantidad();
        }


    }
}