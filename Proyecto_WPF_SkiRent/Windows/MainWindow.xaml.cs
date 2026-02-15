using Proyecto_WPF_SkiRent.Windows;
using SkiRentModel.Repos;
using System.Windows;

namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Ventana principal de la aplicacion.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Inicializa la ventana y carga datos
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            cargarDatos();
        }

        /// <summary>
        /// Carga los datos de los bloques que muestran informacion
        /// de cuantos clientes hay etc , los 3 bloques en el mainwindow
        /// </summary>
        private void cargarDatos()
        {
            var repoCliente = new ClienteRepo();
            txtTotalClientes.Text = repoCliente.Cantidad().ToString();

            var repoMaterial = new MaterialRepo();
            txtTotalMaterial.Text = repoMaterial.Cantidad().ToString();

            var repoAlquiler = new AlquilerRepo();
            txtAlquileresActivos.Text = repoAlquiler.CantidadActivos().ToString();
        }

        /// <summary>
        /// Abre la ventana de clientes
        /// </summary>
        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            WindowClientes w = new WindowClientes();
            w.ShowDialog();
            w.Close();
        }

        /// <summary>
        /// Abre la ventana de material
        /// </summary>
        private void btnMateriales_Click(object sender, RoutedEventArgs e)
        {
            WindowMaterial w = new WindowMaterial();
            w.ShowDialog();
            w.Close();
        }

        /// <summary>
        /// Abre la ventana de alquileres
        /// </summary>
        private void btnAlquileres_Click(object sender, RoutedEventArgs e)
        {
            WindowAlquiler w = new WindowAlquiler();
            w.ShowDialog();
            w.Close();
        }

        /// <summary>
        /// Abre la ventana de categorias.
        /// </summary>
        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            WindowCategorias w = new WindowCategorias();
            w.ShowDialog();
            this.Close();
        }

        private void btnInformeMaterial_Click(object sender, RoutedEventArgs e)
        {
            InformeMaterialWindow w = new InformeMaterialWindow();
            w.ShowDialog();
        }



        private void btnInformAlquileres_Click(object sender, RoutedEventArgs e)
        {
            InformeReservaClienteWindow w = new InformeReservaClienteWindow();
            w.ShowDialog();
        }

        private void btnInformeMaterialEstado_Click(object sender, RoutedEventArgs e)
        {
            InformeMaterialEstado w = new InformeMaterialEstado();
            w.ShowDialog();
        }
    }
}
