using Proyecto_WPF_SkiRent.Controllers;
using SkiRentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Ventana para gestionar clientes 
    /// </summary>
    public partial class WindowClientes : Window
    {
        /// <summary>
        /// controller de cliente
        /// </summary>
        private ClienteAPI controller = new ClienteAPI();

        /// <summary>
        /// Cliente que esta actualmente seleccionado en la pantalla en el datagrid
        /// </summary>
        private Cliente clienteSeleccionado = null;

        /// <summary>
        /// Inicializa la ventana, recarga la lista de clientes y limpia el formulario
        /// </summary>
        public WindowClientes()
        {
            InitializeComponent();
            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Recarga la lista de clientes en el datagrid
        /// </summary>
        private void Recargar()
        {
            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = controller.Listar();
        }

        /// <summary>
        /// Limpia los campos del formulario y resetea la seleccion
        /// </summary>
        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtDni.Text = "";                       
            txtTelefono.Text = "";
            txtEmail.Text = "";
            clienteSeleccionado = null;
        }

        

        /// <summary>
        /// Añade un nuevo cliente usando los valores del formulario, muestra error si hay y recarga la lista
        /// </summary>
        private void btnAnyadir_Click(object sender, RoutedEventArgs e)
        {
            string error = controller.Crear(
                txtNombre.Text,
                txtApellidos.Text,
                txtTelefono.Text,
                txtEmail.Text,
                txtDni.Text
            );

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Edita el cliente seleccionado con los valores del formulario, muestra error si hay y recarga la lista
        /// </summary>
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (clienteSeleccionado == null)
            {
                return;
            }

            string error = controller.Editar(
                clienteSeleccionado.IdCliente,
                txtNombre.Text,
                txtApellidos.Text,
                txtTelefono.Text,
                txtEmail.Text,
                txtDni.Text
            );

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Elimina el cliente seleccionado, muestra error si hay y recarga la lista
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (clienteSeleccionado == null)
            {
                return;
            }


            string error = controller.Eliminar(clienteSeleccionado.IdCliente);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Limpia el formulario y recarga la lista de clientes
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            Recargar();
        }

        /// <summary>
        /// Busca clientes por dni y muestra los resultados en el datagrid
        /// </summary>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = controller.Buscar(txtDniBuscar.Text);
        }

        /// <summary>
        /// Actualiza el formulario con los datos del cliente seleccionado
        /// </summary>
        /// <param name="sender">Control que genero la seleccion</param>
        /// <param name="e">Argumentos del evento de seleccion</param>
        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClientes.SelectedItem == null) return;

            clienteSeleccionado = (Cliente)dgClientes.SelectedItem;

            txtNombre.Text = clienteSeleccionado.Nombre;
            txtApellidos.Text = clienteSeleccionado.Apellidos;
            txtDni.Text = clienteSeleccionado.DNI;
            txtTelefono.Text = clienteSeleccionado.Telefono;
            txtEmail.Text = clienteSeleccionado.Email;
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.ShowDialog();
            this.Close();
        }

        private void btnMaterialesNav_Click(object sender, RoutedEventArgs e)
        {
            WindowMaterial w = new WindowMaterial();
            w.ShowDialog();
            this.Close();
        }

        private void btnCategoriasNav_Click(object sender, RoutedEventArgs e)
        {
            WindowCategorias w = new WindowCategorias();
            w.ShowDialog();
            this.Close();
        }

        private void btnAlquileresNav_Click(object sender, RoutedEventArgs e)
        {
            WindowAlquiler w = new WindowAlquiler();
            w.ShowDialog();
            this.Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClientesNav_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
