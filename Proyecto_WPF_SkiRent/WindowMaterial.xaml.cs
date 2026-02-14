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
    public partial class WindowMaterial : Window
    {
        private MaterialAPI controller = new MaterialAPI();
        private CategoriaRepo categoriaRepo = new CategoriaRepo();

        private Material materialSeleccionado = null;

        public WindowMaterial()
        {
            InitializeComponent();
            CargarCategorias();
            CargarMaterial();
            CargarEstado();
        }

        private void CargarEstado()
        {
            List<string> listaEstados = new List<string>();
            listaEstados.Add("Disponible");
            listaEstados.Add("Mantenimiento");
            listaEstados.Add("Baja");

            cmbEstado.ItemsSource = listaEstados;
            cmbEstado.SelectedIndex = 0;

        }
        private void CargarMaterial()
        {
            dgMaterial.ItemsSource = controller.Listar();
        }

        private void CargarCategorias()
        {
            var categorias = categoriaRepo.Listar();

            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.DisplayMemberPath = "NombreCategoria";
            cmbCategoria.SelectedValuePath = "IdCategoria";
            cmbCategoria.SelectedIndex = -1;
        }

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

        //se usa para pasar los numeros a int si se han dado correctamente y sino da error,
        //lo he puesto en un metodo pq sino se repetia en editar y añadir
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

        private int ObtenerIdCategoriaSeleccionada()
        {
            if (cmbCategoria.SelectedValue == null)
            {
                return 0;
            }

            return (int)cmbCategoria.SelectedValue;
        }

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

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarMaterial();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            dgMaterial.ItemsSource = controller.BuscarPorCodigo(txtCodigoBuscar.Text);
        }

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
