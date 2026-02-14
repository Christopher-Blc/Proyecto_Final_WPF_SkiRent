using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Proyecto_WPF_SkiRent.Controllers
{
    public class ClienteAPI
    {
        private ClienteRepo repo = new ClienteRepo();

        public List<Cliente> Listar()
        {
            return repo.Listar();
        }

        public List<Cliente> Buscar(string texto)
        {
            return repo.BuscarPorDni(texto);
        }

        public Cliente BuscarPorId(int id)
        {
            return repo.BuscarPorId(id);
        }

        //devuelve null si no ha tenido ningun error, o un string con el error si se ha introducido alguna cosa mal
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
        //lo que se usara en el dashboard para mostrar cuantos clientes hay
        public int Cantidad()
        {
            return repo.Cantidad();
        }


    }
}
