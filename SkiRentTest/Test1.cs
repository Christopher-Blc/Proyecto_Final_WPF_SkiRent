using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_WPF_SkiRent.Utils;
using SkiRentModel;
using SkiRentModel.Repos;
using System;
using System.Linq;

namespace SkiRentTest
{
    [TestClass]
    public sealed class Test1
    {
        // Test unitario 1
        //validamos el formato del dni
        [TestMethod]
        public void ValidarDniCliente()
        {
            var c = new Cliente();

            //introducimos datos validos excepto el DNI y miramos que saslte el error que se 
            //ha puesto en la validacion
            c.DNI = "123";
            c.Nombre = "Pepe";
            c.Apellidos = "Prueba";
            c.Telefono = "600000000";
            c.Email = "pepe@correo.com";

            var error = Validaciones.ValidarCliente(
                c.Nombre,
                c.Apellidos,
                c.DNI,
                c.Telefono,
                c.Email
            );

            Assert.AreEqual("Formato de DNI incorrecto.", error);
        }

        // Test unitario 2
        //validar una linea alquiler metiendo una cantidad no valida como 0 
        [TestMethod]
        public void ValidarCantidadLineaAlquiler()
        {
            var l = new LineaAlquiler();

            l.IdAlquiler = 1;
            l.IdMaterial = 1;
            l.Cantidad = 0; // no valida
            l.Dias = 2;

            var error = Validaciones.ValidarLineaAlquiler(
                l.IdAlquiler,
                l.IdMaterial,
                l.Cantidad,
                l.Dias
            );

            Assert.AreEqual("La cantidad debe ser mayor que 0.", error);
        }

        // Test de integracion
        // Guardar cliente en BD, comprobar y borrar
        [TestMethod]
        public void GuardarCliente()
        {
            var repo = new ClienteRepo();

            var c = new Cliente();
            c.DNI = "89940488G";
            c.Nombre = "Test";
            c.Apellidos = "test";
            c.Telefono = "541251848";
            c.Email = "test@test.com";

            //validamos antes de guardar
            var errorValidacion = Validaciones.ValidarCliente(c.Nombre, c.Apellidos, c.DNI, c.Telefono, c.Email);
            Assert.IsNull(errorValidacion , errorValidacion);

            repo.Anyadir(c);

            var encontrado = repo.Listar().FirstOrDefault(x => x.DNI == "89940488G");

            Assert.IsNotNull(encontrado, "El cliente no se ha guardado.");
            //eliminamos el ejemplo para que no se quede en la bbdd
            repo.Eliminar(encontrado.IdCliente);
        }
    }
}
