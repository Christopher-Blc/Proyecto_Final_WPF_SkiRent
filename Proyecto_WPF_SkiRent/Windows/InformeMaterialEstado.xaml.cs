using SkiRentInformes;
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
using System.Windows.Shapes;

namespace Proyecto_WPF_SkiRent.Windows
{
    /// <summary>
    /// Clase que muestra el informe de material agrupado por su estado
    /// </summary>
    public partial class InformeMaterialEstado : Window
    {

        /// <summary>
        /// cargamos el dataset en el informe y lo mostramos en el host
        /// </summary>
        private readonly Helper helper = new Helper();
        public InformeMaterialEstado()
        {
            InitializeComponent();

            var ds = helper.CargarMaterialPorEstado();
            var cr = new CrMaterialEstado();

            cr.SetDataSource(ds);
            host.ReportSource = cr;
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
