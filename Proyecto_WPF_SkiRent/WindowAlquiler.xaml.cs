using Proyecto_WPF_SkiRent.Controllers;
using SkiRentModel;
using SkiRentModel.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_WPF_SkiRent
{
    public partial class WindowAlquiler : Window
    {

        //----------------------A partir de aqui es la parte de la gestion para los alquileres---------------------
        private AlquilerAPI controller = new AlquilerAPI();
        private ClienteRepo clienteRepo = new ClienteRepo();

        private Alquiler alquilerSeleccionado = null;

        //lo gasto mas abajo para el placeholder del buscador, asi si quiero cambiar el texto solo lo cambio aqui
        private const string PlaceholderBuscar = "Buscar por Cliente...";

        public WindowAlquiler()
        {
            InitializeComponent();
            CargarClientes();
            CargarEstados();
            CargarAlquileres();
            LimpiarFormulario();
            CargarMaterialesDisponibles();
            CargarCantidad();
        }

        private void CargarAlquileres()
        {
            dgAlquileres.ItemsSource = controller.Listar();
        }

        private void CargarClientes()
        {
            var clientes = clienteRepo.Listar();

            cmbCodigo.ItemsSource = clientes;
            cmbCodigo.DisplayMemberPath = "DNI";
            cmbCodigo.SelectedValuePath = "IdCliente";
            cmbCodigo.SelectedIndex = -1;
        }

        private void CargarEstados()
        {

            List<string> listaEstado = new List<string>();
            listaEstado.Add("Abierto");
            listaEstado.Add("Cerrado");
            listaEstado.Add("Cancelado");


            cmbEstado.ItemsSource = listaEstado;

            cmbEstado.SelectedIndex = -1;
        }

        private void LimpiarFormulario()
        {
            cmbCodigo.SelectedIndex = -1;
            dtpInicio.SelectedDate = null;
            dtpFin.SelectedDate = null;
            cmbEstado.SelectedIndex = -1;

            txtPrecioTotal.Text = "0";

            alquilerSeleccionado = null;

            if (dgAlquileres != null)
            {
                dgAlquileres.SelectedItem = null;
            }

            dgProductosAlquiler.ItemsSource = null;
            lineaSeleccionada = null;
        }


        private void btnAnyadir_Click(object sender, RoutedEventArgs e)
        {
            //hacemos tantos ifs para tener un mensaje personalizado antes de uno que diga rellena todos los campos
            if (cmbCodigo.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un cliente.");
                return;
            }

            if (dtpInicio.SelectedDate == null || dtpFin.SelectedDate == null)
            {
                MessageBox.Show("Selecciona fecha inicio y fecha fin.");
                return;
            }

            if (cmbEstado.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un estado.");
                return;
            }

            int idCliente = (int)cmbCodigo.SelectedValue;
            DateTime inicio = dtpInicio.SelectedDate.Value;
            DateTime fin = dtpFin.SelectedDate.Value;
            string estado = cmbEstado.SelectedItem.ToString();

            string error = controller.Crear(idCliente, inicio, fin, estado);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarAlquileres();
            LimpiarFormulario();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (alquilerSeleccionado == null)
            {
                MessageBox.Show("Selecciona un alquiler.");
                return;
            }

            if (cmbCodigo.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un cliente.");
                return;
            }

            if (dtpInicio.SelectedDate == null || dtpFin.SelectedDate == null)
            {
                MessageBox.Show("Selecciona fecha inicio y fecha fin.");
                return;
            }

            if (cmbEstado.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un estado.");
                return;
            }

            int idCliente = (int)cmbCodigo.SelectedValue;
            DateTime inicio = dtpInicio.SelectedDate.Value;
            DateTime fin = dtpFin.SelectedDate.Value;
            string estado = cmbEstado.SelectedItem.ToString();

            string error = controller.Editar(alquilerSeleccionado.IdAlquiler, idCliente, inicio, fin, estado);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarAlquileres();
            LimpiarFormulario();
        }


        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (alquilerSeleccionado == null)
            {
                MessageBox.Show("Selecciona un alquiler.");
                return;
            }

            string error = controller.Eliminar(alquilerSeleccionado.IdAlquiler);

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarAlquileres();
            LimpiarFormulario();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarAlquileres();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = (txtBuscarId.Text ?? "").Trim();

            if (texto == PlaceholderBuscar)
            {
                texto = "";
            }

            dgAlquileres.ItemsSource = controller.Buscar(texto);
        }

        private void dgAlquileres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAlquileres.SelectedItem == null)
            {
                return;
            }

            alquilerSeleccionado = (Alquiler)dgAlquileres.SelectedItem;

            cmbCodigo.SelectedValue = alquilerSeleccionado.IdCliente;
            dtpInicio.SelectedDate = alquilerSeleccionado.FechaInicio;
            dtpFin.SelectedDate = alquilerSeleccionado.FechaFin;
            cmbEstado.Text = alquilerSeleccionado.Estado;

            txtPrecioTotal.Text = alquilerSeleccionado.Total.ToString();
            CargarLineasDelAlquiler();
        }

        private void txtBuscarId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscarId.Text == PlaceholderBuscar)
            {
                txtBuscarId.Text = "";
                txtBuscarId.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void txtBuscarId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarId.Text))
            {
                txtBuscarId.Text = PlaceholderBuscar;
                txtBuscarId.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        //botones de cambiar de pantalla
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

        private void btnCategoriasNav_Click(object sender, RoutedEventArgs e)
        {
            WindowCategorias w = new WindowCategorias();
            w.ShowDialog();
            this.Close();
        }


        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //___________________________Aqui ya esta la parte derecha de la ventana para añadir lineas de pedido al pedido__________________

        private LineaAlquilerAPI lineaController = new LineaAlquilerAPI();
        private LineaAlquiler lineaSeleccionada = null;
        private void btnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (alquilerSeleccionado == null)
            {
                MessageBox.Show("Selecciona un alquiler.");
                return;
            }

            if (cmbMaterialAlquiler.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un material.");
                return;
            }

            if (cmbCantidad.SelectedItem == null)
            {
                MessageBox.Show("Selecciona cantidad.");
                return;
            }

            int diasAlquiler = ObtenerDiasAlquiler();

            int idMaterial = (int)cmbMaterialAlquiler.SelectedValue;
            int cantidad = (int)cmbCantidad.SelectedItem;
            int dias = diasAlquiler;

            string error = lineaController.AnyadirLinea(alquilerSeleccionado.IdAlquiler, idMaterial, cantidad, dias);
            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            CargarLineasDelAlquiler();// refresca panel derecho
            CargarAlquileres();// refresca totales
            CargarMaterialesDisponibles();

            //limpiar
            cmbMaterialAlquiler.SelectedIndex = -1;
            cmbCantidad.SelectedIndex = 0;

        }

        private void dgProductosAlquiler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductosAlquiler.SelectedItem == null)
            {
                lineaSeleccionada = null;
                return;
            }

            lineaSeleccionada = (LineaAlquiler)dgProductosAlquiler.SelectedItem;
        }


        private void CargarLineasDelAlquiler()
        {
            if (alquilerSeleccionado == null)
            {
                dgProductosAlquiler.ItemsSource = null;
                return;
            }

            dgProductosAlquiler.ItemsSource = lineaController.ListarPorAlquiler(alquilerSeleccionado.IdAlquiler);
        }


        private void CargarMaterialesDisponibles()
        {
            var materiales = lineaController.ListarMaterialesDisponibles();

            cmbMaterialAlquiler.ItemsSource = materiales;
            cmbMaterialAlquiler.DisplayMemberPath = "Codigo";     
            cmbMaterialAlquiler.SelectedValuePath = "IdMaterial";
            cmbMaterialAlquiler.SelectedIndex = -1;
        }

        private void CargarCantidad()
        {
            List<int> cantidades = new List<int>();
            for (int i = 1; i <= 10; i++)
            {
                cantidades.Add(i);
            }

            cmbCantidad.ItemsSource = cantidades;
            cmbCantidad.SelectedIndex = 0;
        }

        private void btnQuitarProducto_Click(object sender, RoutedEventArgs e)
        {
        
            if (alquilerSeleccionado == null)
            {
                MessageBox.Show("Selecciona un alquiler.");
                return;
            }

            if (lineaSeleccionada == null)
            {
                MessageBox.Show("Selecciona un producto del alquiler.");
                return;
            }

            string error = lineaController.EliminarLinea(lineaSeleccionada.IdLinea);
            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            lineaSeleccionada = null;

            CargarLineasDelAlquiler();
            CargarAlquileres();            
            CargarMaterialesDisponibles(); 
        }

        //Metodo que calcula los dias que tarda el alquiler(reserva) 
        //lo calcula a partir de la fecha de inicio hasta la de fin.
        private int ObtenerDiasAlquiler()
        {
            if (dtpInicio.SelectedDate == null || dtpFin.SelectedDate == null)
            {
                return 0;
            }

            DateTime inicio = dtpInicio.SelectedDate.Value.Date;
            DateTime fin = dtpFin.SelectedDate.Value.Date;

            int dias = (fin - inicio).Days;

            // si quieres que mismo día cuente como 1 día
            if (dias <= 0)
            {
                dias = 1;
            }

            return dias;
        }

    }
}
