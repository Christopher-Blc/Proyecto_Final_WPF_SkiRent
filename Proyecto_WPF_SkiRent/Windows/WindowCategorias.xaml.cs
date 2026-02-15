using Proyecto_WPF_SkiRent.Controllers;
using SkiRentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Ventana para gestionar categorias de material
    /// </summary>
    public partial class WindowCategorias : Window
    {
        /// <summary>
        /// controller de categorias
        /// </summary>
        private CategoriaAPI controller = new CategoriaAPI();

        /// <summary>
        /// Guarda la categoria actualmente seleccionada en la pantalla
        /// </summary>
        private CategoriaMaterial categoriaSeleccionada = null;

        /// <summary>
        /// Inicializa componentes, carga niveles y categorias
        /// </summary>
        public WindowCategorias()
        {
            InitializeComponent();
            CargarNiveles();
            CargarCategorias();
        }

        /// <summary>
        /// Llena el combo de niveles con opciones y deja sin seleccion
        /// </summary>
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

        /// <summary>
        /// Carga la lista de categorias en el datagrid
        /// </summary>
        private void CargarCategorias()
        {
            dgCategorias.ItemsSource = controller.Listar();
        }

        /// <summary>
        /// Limpia los campos del formulario y resetea la seleccion
        /// </summary>
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

        /// <summary>
        /// Toma los datos del formulario y crea una nueva categoria, muestra mensaje si hay error
        /// </summary>
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

        /// <summary>
        /// Actualiza la categoria seleccionada con los valores del formulario, muestra mensaje si hay error
        /// </summary>
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

        /// <summary>
        /// Elimina la categoria seleccionada por id, muestra mensaje si hay error
        /// </summary>
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

        /// <summary>
        /// Limpia formulario y recarga las categorias
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarCategorias();
        }

        /// <summary>
        /// Busca categorias que coinciden con el texto de busqueda y las muestra en el datagrid
        /// </summary>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            dgCategorias.ItemsSource = controller.Buscar(txtBuscar.Text);
        }

        /// <summary>
        /// Actualiza el formulario con los datos de la fila seleccionada en el datagrid
        /// </summary>
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
