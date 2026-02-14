using Proyecto_WPF_SkiRent.Controllers;
using SkiRentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_WPF_SkiRent
{
    public partial class WindowClientes : Window
    {
        private ClienteAPI controller = new ClienteAPI();
        private Cliente clienteSeleccionado = null;

        public WindowClientes()
        {
            InitializeComponent();
            Recargar();
            LimpiarFormulario();
        }

        private void Recargar()
        {
            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = controller.Listar();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtDni.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            clienteSeleccionado = null;
        }

        

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

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            Recargar();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = controller.Buscar(txtDniBuscar.Text);
        }

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
