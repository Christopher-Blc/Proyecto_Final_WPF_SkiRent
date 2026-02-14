using SkiRentModel.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cargarDatos();
        }



        private void cargarDatos()
        {
            var repoCliente = new ClienteRepo();
            txtTotalClientes.Text = repoCliente.Cantidad().ToString();

            var repoMaterial = new MaterialRepo();
            txtTotalMaterial.Text = repoMaterial.Cantidad().ToString();

            var repoAlquiler = new AlquilerRepo();
            txtAlquileresActivos.Text = repoAlquiler.CantidadActivos().ToString();
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            WindowClientes w = new WindowClientes();
            w.ShowDialog();
            w.Close();
        }

        private void btnMateriales_Click(object sender, RoutedEventArgs e)
        {
            WindowMaterial w = new WindowMaterial();
            w.ShowDialog();
            w.Close();
        }

        private void btnAlquileres_Click(object sender, RoutedEventArgs e)
        {
            WindowAlquiler w = new WindowAlquiler();
            w.ShowDialog();
            w.Close();
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            WindowCategorias w = new WindowCategorias();
            w.ShowDialog();
            w.Close();
        }
    }
}
