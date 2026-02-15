using Proyecto_WPF_SkiRent.Controllers;
using SkiRentModel;
using SkiRentModel.Repos;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Ventana para gestionar materiales
    /// </summary>
    public partial class WindowMaterial : Window
    {
        /// <summary>
        /// controller de material
        /// </summary>
        private MaterialAPI controller = new MaterialAPI();

        /// <summary>
        /// repode categoria que gastaremos para mostrar las categorias en el combo y obtener su id
        /// </summary>
        private CategoriaRepo categoriaRepo = new CategoriaRepo();

        /// <summary>
        /// Material seleccionado en la pantalla
        /// </summary>
        private Material materialSeleccionado = null;

        /// <summary>
        /// Inicia la ventana y carga las listas necesarias
        /// </summary>
        public WindowMaterial()
        {
            InitializeComponent();
            CargarCategorias();
            CargarMaterial();
            CargarEstado();
        }

        /// <summary>
        /// Crea la lista de estados posibles y la asigna al combo
        /// </summary>
        private void CargarEstado()
        {
            List<string> listaEstados = new List<string>();
            listaEstados.Add("Disponible");
            listaEstados.Add("Mantenimiento");
            listaEstados.Add("Baja");

            cmbEstado.ItemsSource = listaEstados;
            cmbEstado.SelectedIndex = 0;

        }   

        /// <summary>
        /// Carga todos los materiales en el datagrid
        /// </summary>
        private void CargarMaterial()
        {
            dgMaterial.ItemsSource = controller.Listar();
        }

        /// <summary>
        /// Carga las categorias en el combobox mostrando su nombre
        /// </summary>
        private void CargarCategorias()
        {
            var categorias = categoriaRepo.Listar();

            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.DisplayMemberPath = "NombreCategoria";
            cmbCategoria.SelectedValuePath = "IdCategoria";
            cmbCategoria.SelectedIndex = -1;
        }

        /// <summary>
        /// Limpia los controles del formulario y resetea la seleccion
        /// </summary>
        private void LimpiarFormulario()
        {
            txtCodigo.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtStock.Text = "";
            txtTallaLongitud.Text = "";
            cmbEstado.SelectedIndex = -1;
            txtPrecioDia.Text = "";
            cmbCategoria.SelectedIndex = 0;

            materialSeleccionado = null;
        }

        /// <summary>
        /// Intenta convertir los textos a numero , muestra mensaje si hay error
        /// </summary>
        /// <param name="precioDia">precio convertido por dia</param>
        /// <param name="stock">stock convertido</param>
        /// <returns>verdadero si la conversion fue correcta, falso si habia un error</returns>
        private bool ConvertirNumeros(out decimal precioDia, out int stock)
        {
            precioDia = 0;
            stock = 0;

            if (!decimal.TryParse(txtPrecioDia.Text, out precioDia))
            {
                MessageBox.Show("Precio invalido.");
                return false;
            }

            if (!int.TryParse(txtStock.Text, out stock))
            {
                MessageBox.Show("Stock invalido.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Devuelve el id de la categoria seleccionada o 0 si no hay nada seleccionado
        /// </summary>
        /// <returns>id de la categoria o 0</returns>
        private int ObtenerIdCategoriaSeleccionada()
        {
            if (cmbCategoria.SelectedValue == null)
            {
                return 0;
            }

            return (int)cmbCategoria.SelectedValue;
        }

        /// <summary>
        /// Añade un nuevo material usando los datos del formulario
        /// </summary>
        private void btnAnyadir_Click(object sender, RoutedEventArgs e)
        {
            if (!ConvertirNumeros(out decimal precioDia, out int stock))
            {
                return;
            }

            //obtenemos los datos de los cmb
            int idCategoria = ObtenerIdCategoriaSeleccionada();
            string estado = cmbEstado.SelectedItem?.ToString();

            string error = controller.Crear(
                txtCodigo.Text,
                txtMarca.Text,
                txtModelo.Text,
                txtTallaLongitud.Text,
                estado,
                precioDia,
                stock,
                idCategoria
            );

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarMaterial();
            LimpiarFormulario();
        }


        /// <summary>
        /// Edita el material seleccionado con los datos del formulario
        /// </summary>
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (materialSeleccionado == null)
            {
                MessageBox.Show("Ningun material seleccionado");
                return;
            }

            if (!ConvertirNumeros(out decimal precioDia, out int stock))
            {
                return;
            }

            int idCategoria = ObtenerIdCategoriaSeleccionada();
            string estado = cmbEstado.SelectedItem?.ToString();

            string error = controller.Editar(
                materialSeleccionado.IdMaterial,
                txtCodigo.Text,
                txtMarca.Text,
                txtModelo.Text,
                txtTallaLongitud.Text,
                estado,
                precioDia,
                stock,
                idCategoria
            );

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarMaterial();
            LimpiarFormulario();
        }


        /// <summary>
        /// Elimina el material seleccionado
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (materialSeleccionado == null)
            {
                MessageBox.Show("Ningun material seleccionado");
                return;
            }



            string error = controller.Eliminar(materialSeleccionado.IdMaterial);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarMaterial();
            LimpiarFormulario();
        }

        /// <summary>
        /// Limpia el formulario y recarga la lista
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarMaterial();
        }

        /// <summary>
        /// Busca por codigo y muestra los resultados en el datagrid
        /// </summary>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            dgMaterial.ItemsSource = controller.BuscarPorCodigo(txtCodigoBuscar.Text);
        }

        /// <summary>
        /// Al cambiar la seleccion, rellena el formulario con los datos del material
        /// </summary>
        private void dgMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMaterial.SelectedItem == null)
            {
                return;
            }

            materialSeleccionado = (Material)dgMaterial.SelectedItem;

            txtCodigo.Text = materialSeleccionado.Codigo;
            txtMarca.Text = materialSeleccionado.Marca;
            txtModelo.Text = materialSeleccionado.Modelo;
            txtTallaLongitud.Text = materialSeleccionado.TallaLongitud;
            cmbEstado.SelectedItem = materialSeleccionado.Estado;

            txtPrecioDia.Text = materialSeleccionado.PrecioDia.ToString(CultureInfo.InvariantCulture);
            txtStock.Text = materialSeleccionado.Stock.ToString();

            cmbCategoria.SelectedValue = materialSeleccionado.IdCategoria;
        }


        //Botones de cambiar de ventana
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.ShowDialog();
            Close();
        }

        private void btnClientesNav_Click(object sender, RoutedEventArgs e)
        {
            WindowClientes w = new WindowClientes();
            w.ShowDialog();
            Close();
        }

        private void btnCategoriasNav_Click(object sender, RoutedEventArgs e)
        {
            WindowCategorias w = new WindowCategorias();
            w.ShowDialog();
            Close();
        }

        private void btnAlquileresNav_Click(object sender, RoutedEventArgs e)
        {
            WindowAlquiler w = new WindowAlquiler();
            w.ShowDialog();
            Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
