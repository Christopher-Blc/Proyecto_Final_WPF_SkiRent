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
    
    /// <summary>
    /// Ventana para gestionar alquileres
    /// </summary>
    public partial class WindowAlquiler : Window
    {

        /// <summary>
        /// controller de alquiler
        /// </summary>
        private AlquilerAPI controller = new AlquilerAPI();

        /// <summary>
        /// controller de cliente
        /// </summary>
        private ClienteRepo clienteRepo = new ClienteRepo();

        /// <summary>
        /// nos guarda el alquiler que selecciona el usuario en la pantalla
        /// </summary>
        private Alquiler alquilerSeleccionado = null;

        /// <summary>
        /// Texto placeholder para el buscador
        /// </summary>
        private const string PlaceholderBuscar = "Buscar por Cliente...";

        /// <summary>
        /// controler de las lineas de alquiler
        /// </summary>
        private LineaAlquilerAPI lineaController = new LineaAlquilerAPI();

        /// <summary>
        /// nos guarda la linea(el producto) que selecciona el usuario en la pantalla
        /// </summary>
        private LineaAlquiler lineaSeleccionada = null;

        /// <summary>
        /// Inicializa la ventana y carga datos como DG y combobox
        /// </summary>
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

        /// <summary>
        /// Carga la lista de alquileres y la asigna al datagrid
        /// </summary>
        private void CargarAlquileres()
        {
            dgAlquileres.ItemsSource = controller.Listar();
        }

        /// <summary>
        /// Carga los clientes en el combobox de dni
        /// </summary>
        private void CargarClientes()
        {
            var clientes = clienteRepo.Listar();

            cmbCodigo.ItemsSource = clientes;
            cmbCodigo.DisplayMemberPath = "DNI";
            cmbCodigo.SelectedValuePath = "IdCliente";
            cmbCodigo.SelectedIndex = -1;
        }

        /// <summary>
        /// Carga los estados posibles para un alquiler
        /// </summary>
        private void CargarEstados()
        {

            List<string> listaEstado = new List<string>();
            listaEstado.Add("Abierto");
            listaEstado.Add("Cerrado");
            listaEstado.Add("Cancelado");


            cmbEstado.ItemsSource = listaEstado;
            cmbEstado.SelectedIndex = -1;
        }

        /// <summary>
        /// Limpia el formulario y devuelve valores por defecto
        /// </summary>
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


        /// <summary>
        /// Crea un nuevo alquiler validando los campos y recarga la lista
        /// </summary>
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

        /// <summary>
        /// Edita el alquiler seleccionado validando los campos y recarga la lista
        /// </summary>
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


        /// <summary>
        /// Elimina el alquiler seleccionado y recarga la lista
        /// </summary>
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

        /// <summary>
        /// Limpia el formulario y actualiza la lista de alquileres
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            CargarAlquileres();
        }

        /// <summary>
        /// Realiza una busqueda por cliente y muestra los resultados en el datagrid
        /// </summary>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = (txtBuscarId.Text ?? "").Trim();

            if (texto == PlaceholderBuscar)
            {
                texto = "";
            }

            dgAlquileres.ItemsSource = controller.Buscar(texto);
        }

        /// <summary>
        /// Carga los datos del alquiler seleccionado en el formulario y muestra sus lineas
        /// </summary>
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

        /// <summary>
        /// Quita el texto placeholder y pone el color negro si el texto coincide con el placeholder
        /// </summary>
        private void txtBuscarId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscarId.Text == PlaceholderBuscar)
            {
                txtBuscarId.Text = "";
                txtBuscarId.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        /// <summary>
        /// Restaura el texto placeholder y pone color gris si el campo esta vacio
        /// </summary>
        private void txtBuscarId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarId.Text))
            {
                txtBuscarId.Text = PlaceholderBuscar;
                txtBuscarId.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        //botones de cambiar de pantalla

        /// <summary>
        /// Abre la ventana principal y cierra la actual
        /// </summary>
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Abre la ventana de clientes y cierra la actual
        /// </summary>
        private void btnClientesNav_Click(object sender, RoutedEventArgs e)
        {
            WindowClientes w = new WindowClientes();
            w.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Abre la ventana de materiales y cierra la actual
        /// </summary>
        private void btnMaterialesNav_Click(object sender, RoutedEventArgs e)
        {
            WindowMaterial w = new WindowMaterial();
            w.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Abre la ventana de categorias y cierra la actual
        /// </summary>
        private void btnCategoriasNav_Click(object sender, RoutedEventArgs e)
        {
            WindowCategorias w = new WindowCategorias();
            w.ShowDialog();
            this.Close();
        }


        /// <summary>
        /// Cierra la ventana actual
        /// </summary>
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //___________________________Aqui ya esta la parte derecha de la ventana para añadir lineas de pedido al pedido__________________


        /// <summary>
        /// Anade un material al alquiler comprobando campos, recarga lineas y materiales disponibles
        /// </summary>
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

        /// <summary>
        /// Guarda la linea seleccionada o la pone a null si no hay seleccion
        /// </summary>
        private void dgProductosAlquiler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductosAlquiler.SelectedItem == null)
            {
                lineaSeleccionada = null;
                return;
            }

            lineaSeleccionada = (LineaAlquiler)dgProductosAlquiler.SelectedItem;
        }


        /// <summary>
        /// Carga las lineas del alquiler seleccionado en el grid lateral
        /// </summary>
        private void CargarLineasDelAlquiler()
        {
            if (alquilerSeleccionado == null)
            {
                dgProductosAlquiler.ItemsSource = null;
                return;
            }

            dgProductosAlquiler.ItemsSource = lineaController.ListarPorAlquiler(alquilerSeleccionado.IdAlquiler);
        }


        /// <summary>
        /// Carga los materiales disponibles para anyadir a un alquiler
        /// </summary>
        private void CargarMaterialesDisponibles()
        {
            var materiales = lineaController.ListarMaterialesDisponibles();

            cmbMaterialAlquiler.ItemsSource = materiales;
            cmbMaterialAlquiler.DisplayMemberPath = "Codigo";     
            cmbMaterialAlquiler.SelectedValuePath = "IdMaterial";
            cmbMaterialAlquiler.SelectedIndex = -1;
        }

        /// <summary>
        /// Carga la lista de cantidades que se pueden seleccionar
        /// </summary>
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

        /// <summary>
        /// Quita la linea seleccionada del alquiler y recarga los datos relacionados
        /// </summary>
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

        /// <summary>
        /// Calcula los dias del alquiler entre fecha inicio y fecha fin
        /// </summary>
        /// <return>numero de dias</return>
        private int ObtenerDiasAlquiler()
        {
            if (dtpInicio.SelectedDate == null || dtpFin.SelectedDate == null)
            {
                return 0;
            }

            DateTime inicio = dtpInicio.SelectedDate.Value.Date;
            DateTime fin = dtpFin.SelectedDate.Value.Date;

            int dias = (fin - inicio).Days;

            // si quieres que mismo dia cuente como 1 dia
            if (dias <= 0)
            {
                dias = 1;
            }

            return dias;
        }

    }
}
