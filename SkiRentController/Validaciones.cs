using SkiRentModel.Repos;
using System;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

namespace Proyecto_WPF_SkiRent.Utils
{
    /// <summary>
    /// Metodos para validar datos de clientes, material, categorias y alquileres.
    /// </summary>
    public static class Validaciones
    {
        /// <summary>
        /// Valida los datos basicos de un cliente.
        /// </summary>
        /// <param name="nombre">Nombre del cliente.</param>
        /// <param name="apellidos">Apellidos del cliente.</param>
        /// <param name="dni">Documento nacional de identidad, formato 8 digitos y letra.</param>
        /// <param name="telefono">Telefono del cliente, 9 digitos.</param>
        /// <param name="email">Correo electronico del cliente.</param>
        /// <returns>Mensaje de error si hay un fallo, o null si todo es valido.</returns>
        public static string ValidarCliente(string nombre, string apellidos, string dni, string telefono, string email)
        {

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(apellidos))
            {
                return "Los apellidos son obligatorios.";
            }

            if (string.IsNullOrWhiteSpace(dni))
            {
                return "El DNI es obligatorio.";
            }

            if (!Regex.IsMatch(dni.Trim().ToUpper(), @"^\d{8}[A-Z]$"))
            {
                return "Formato de DNI incorrecto.";
            }

            if (string.IsNullOrWhiteSpace(telefono))
            {
                return "El teléfono es obligatorio.";
            }

            if (!Regex.IsMatch(telefono.Trim(), @"^\d{9}$"))
            {
                return "El teléfono debe tener 9 dígitos.";
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return "El email es obligatorio.";
            }

            if (!Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return "Formato de email incorrecto.";
            }



            return null;
        }



        //validaciones para el controller de material

        /// <summary>
        /// Valida los campos de un material antes de guardarlo o editarlo.
        /// </summary>
        /// <param name="codigo">Codigo unico del material.</param>
        /// <param name="marca">Marca del material.</param>
        /// <param name="modelo">Modelo del material.</param>
        /// <param name="talla">Talla del material.</param>
        /// <param name="estado">Estado del material.</param>
        /// <param name="precioDia">Precio por dia del alquiler.</param>
        /// <param name="stock">Cantidad disponible en stock.</param>
        /// <param name="idCategoria">Identificador de la categoria asociada.</param>
        /// <returns>Mensaje de error si hay un fallo, o null si todo es valido.</returns>
        public static string ValidarMaterial(string codigo,string marca,string modelo,string talla,string estado,decimal precioDia,int stock,int idCategoria)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return "El codigo es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(marca))
            {
                return "La marca es obligatoria.";
            }

            if (string.IsNullOrWhiteSpace(modelo))
            {
                return "El modelo es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                return "El estado es obligatorio.";
            }

            if (precioDia <= 0)
            {
                return "El precio debe ser mayor que 0.";
            }

            if (stock < 0)
            {
                return "El stock no puede ser negativo.";
            }

            if (idCategoria <= 0)
            {
                return "Tienes que seleccionar una categoria.";
            }

            return null;
        }

        //validaciopnes para las categorias

        /// <summary>
        /// Valida los datos de una categoria.
        /// </summary>
        /// <param name="nombreCategoria">Nombre de la categoria.</param>
        /// <param name="nivel">Nivel asociado a la categoria.</param>
        /// <returns>Mensaje de error si hay un fallo, o null si todo es valido.</returns>
        public static string ValidarCategoria(string nombreCategoria, string nivel)
        {

            if (string.IsNullOrWhiteSpace(nombreCategoria))
            {
                return "El nombre de categoria es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(nivel))
            {
                return "El nivel es obligatorio.";
            }

            return null;
        }

        //para validar los alquileres

        /// <summary>
        /// Valida los datos basicos de un alquiler.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <param name="fechaInicio">Fecha de inicio del alquiler.</param>
        /// <param name="fechaFin">Fecha de fin del alquiler.</param>
        /// <param name="estado">Estado del alquiler (Abierto, Cerrado, Cancelado).</param>
        /// <returns>Mensaje de error si hay un fallo, o null si todo es valido.</returns>
        public static string ValidarAlquiler(int idCliente, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            if (idCliente <= 0)
            {
                return "Cliente no valido.";
            }

            if (fechaInicio == DateTime.MinValue)
            {
                return "Fecha inicio no valida.";
            }

            // Prevent reservations that start before today
            if (fechaInicio.Date < DateTime.Today)
            {
                return "La fecha de inicio no puede ser anterior a hoy.";
            }

            if (fechaFin == DateTime.MinValue)
            {
                return "Fecha fin no valida.";
            }

            if (fechaFin < fechaInicio)
            {
                return "La fecha fin no puede ser menor que la fecha inicio.";
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                return "Debe seleccionar un estado.";
            }

            string estadoLimpiado = estado.Trim();

            if (estadoLimpiado != "Abierto" && estadoLimpiado != "Cerrado" && estadoLimpiado != "Cancelado")
            {
                return "Estado no valido.";
            }

            return null;
        }


        //validaciopnes para alquiler linea

        /// <summary>
        /// Valida una linea de alquiler que relaciona un material con un alquiler.
        /// </summary>
        /// <param name="idAlquiler">Identificador del alquiler.</param>
        /// <param name="idMaterial">Identificador del material.</param>
        /// <param name="cantidad">Cantidad de unidades.</param>
        /// <param name="dias">Numero de dias de alquiler.</param>
        /// <returns>Mensaje de error si hay un fallo, o null si todo es valido.</returns>
        public static string ValidarLineaAlquiler(int idAlquiler, int idMaterial, int cantidad, int dias)
        {
            if (idAlquiler <= 0)
            {
                return "Selecciona un alquiler.";
            }

            if (idMaterial <= 0)
            {
                return "Selecciona un material.";
            }

            if (cantidad <= 0)
            {
                return "La cantidad debe ser mayor que 0.";
            }

            if (dias <= 0)
            {
                return "Los días deben ser mayor que 0.";
            }

            return null;
        }



    }
}
