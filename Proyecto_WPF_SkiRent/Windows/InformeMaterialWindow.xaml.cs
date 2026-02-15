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
using CrystalDecisions.CrystalReports.Engine;
using SkiRentInformes;

namespace Proyecto_WPF_SkiRent.Windows
{
    /// <summary>
    /// Clase que muestra el informe de material
    /// </summary>
    public partial class InformeMaterialWindow : Window
    {
        private readonly Helper helper = new Helper();

        /// <summary>
        /// cargamos el dataset en el informe y lo mostramos en el host
        /// </summary>
        public InformeMaterialWindow()
        {
            InitializeComponent();

            var ds = helper.CargarDatosMaterial(); 
            var cr = new CrMaterial();      

            cr.SetDataSource(ds);
            host.ReportSource = cr;
        }

        private void WindowsFormsHost_ChildChanged(object sender,
            System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }
    }
}
