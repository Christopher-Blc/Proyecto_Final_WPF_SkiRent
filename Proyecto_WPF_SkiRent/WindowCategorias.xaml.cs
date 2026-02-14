using Proyecto_WPF_SkiRent.Controllers;
using SkiRentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Interaction logic for WindowCategorias.xaml
    /// </summary>
    public partial class WindowCategorias : Window
    {
        private CategoriaAPI controller = new CategoriaAPI();
        private CategoriaMaterial categoriaSeleccionada = null;

        public WindowCategorias()
        {
            InitializeComponent();
            CargarNiveles();
            CargarCategorias();
        }

        private void CargarNiveles()
        {
            cmbNivel.ItemsSource = new List<string>
            {
                "Infantil",
                "Principiante",
                "Intermedio",
                "Avanzado",
                "Profesional"
            };

            cmbNivel.SelectedIndex = -1;
        }

        private void CargarCategorias()
        {
            dgCategorias.ItemsSource = controller.Listar();
        }

        private void LimpiarFormulario()
        {
            txtNombreCategoria.Text = "";
            cmbNivel.SelectedIndex = -1;
            categoriaSeleccionada = null;

            if (dgCategorias != null)
            {
                dgCategorias.SelectedItem = null;
            }
        }

        private void btnAnyadir_Click(object sender, RoutedEventArgs e)
        {
            string nivel = cmbNivel.SelectedItem?.ToString();

            string error = controller.Crear(txtNombreCategoria.Text, nivel);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarCategorias();
            LimpiarFormulario();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (categoriaSeleccionada == null)
            {
                MessageBox.Show("Ninguna categoria seleccionada.");
                return;
            }

            string nivel = cmbNivel.SelectedItem?.ToString();

            string error = controller.Editar(
                categoriaSeleccionada.IdCategoria,
                txtNombreCategoria.Text,
                nivel
            );

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarCategorias();
            LimpiarFormulario();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (categoriaSeleccionada == null)
            {
                MessageBox.Show("Ninguna categoria seleccionada.");
                return;
            }

            string error = controller.Eliminar(categoriaSeleccionada.IdCategoria);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarCategorias();
            LimpiarFormulario();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarCategorias();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            dgCategorias.ItemsSource = controller.Buscar(txtBuscar.Text);
        }

        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategorias.SelectedItem == null)
            {
                return;
            }

            categoriaSeleccionada = (CategoriaMaterial)dgCategorias.SelectedItem;

            txtNombreCategoria.Text = categoriaSeleccionada.NombreCategoria;
            cmbNivel.SelectedItem = categoriaSeleccionada.Nivel;
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.ShowDialog();
            this.Close();
        }

        private void btnClientesNav_Click(object sender, RoutedEventArgs e)
        {
            WindowClientes w = new WindowClientes();
            w.ShowDialog();
            this.Close();
        }

        private void btnMaterialesNav_Click(object sender, RoutedEventArgs e)
        {
            WindowMaterial w = new WindowMaterial();
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


    }
}
