using SkiRentInformes.Datasets;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiRentInformes;


namespace Proyecto_WPF_SkiRent
{
    /// <summary>
    /// Clase que contiene metodos para cargar datos desde la base de datos.
    /// Abre laconexion, ejecuta una consulta y rellena los datasets para usarlos en los informes.
    /// </summary>
    internal class Helper
    {
        /// <summary>
        /// Cadena deconexion usada para abrir la base de datos local.
        /// la he metido aqui para que todos los metodos la usen.
        /// </summary>
        private string conexion = "Server=CHRIS-PC\\SQLEXPRESS;Database=SkiRent;Trusted_Connection=True;";

        /// <summary>
        /// Carga los datos de la tabla Material junto con su categoria.
        /// Abre la conexion, ejecuta la consulta, recorre el lector y añade cada fila al dataset.
        /// </summary>
        /// <returns>DsMaterial con las filas de material y su categoria.</returns>
        public DsMaterial CargarDatosMaterial()
        {
            var ds = new DsMaterial();

            string sql = @"SELECT m.Codigo, m.Marca, m.Modelo, m.TallaLongitud, m.Estado, m.PrecioDia,m.Stock,c.NombreCategoria AS Categoria
                        FROM Material m
                        INNER JOIN CategoriaMaterial c ON c.IdCategoria = m.IdCategoria
                        ORDER BY c.NombreCategoria";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                //abrimos conexion y le metemos los datos al DT del dataset 
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ds.DTMaterial.Rows.Add(
                            dr["Codigo"],
                            dr["Marca"],
                            dr["Modelo"],
                            dr["TallaLongitud"],
                            dr["Estado"],
                            dr["PrecioDia"],
                            dr["Stock"],
                            dr["Categoria"]
                        );
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// Carga las reservas junto con el nombre del cliente.
        /// Ejecuta una consulta que une Alquiler con Cliente, recorre el lector y añade filas al dataset.
        /// </summary>
        /// <returns>DsReservasCliente con las reservas y el nombre del cliente.</returns>
        public DsReservasCliente CargarDatosReservasPorCliente()
        {
            var ds = new DsReservasCliente();
            
            string sql = @"SELECT  a.IdAlquiler,a.IdCliente,
                        (c.Nombre + ' ' + c.Apellidos) AS Nombre,a.FechaInicio, a.FechaFin,a.Estado,a.Total
                        FROM Alquiler a
                        INNER JOIN Cliente c ON c.IdCliente = a.IdCliente
                        ORDER BY Nombre DESC;";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ds.DTReservasCliente.Rows.Add(
                            dr["IdAlquiler"],
                            dr["Nombre"],
                            dr["FechaInicio"],
                            dr["FechaFin"],
                            dr["Estado"],
                            dr["Total"]
                        );
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// Carga el material agrupado y ordenado por su estado y categoria.
        /// Abre la conexion, ejecuta la consulta y rellena el DTMaterialEstado con cada fila leida.
        /// </summary>
        /// <returns>DsMaterialEstado con la informacion del material y su estado.</returns>
        public DsMaterialEstado CargarMaterialPorEstado()
        {
            var ds = new DsMaterialEstado();

            string sql = @"SELECT  m.Estado,m.Codigo, m.Marca, m.Modelo, m.TallaLongitud,c.NombreCategoria AS Categoria,m.Stock,m.PrecioDia
                        FROM Material m
                        INNER JOIN CategoriaMaterial c ON c.IdCategoria = m.IdCategoria
                        ORDER BY m.Estado, c.NombreCategoria, m.Marca, m.Modelo;";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ds.DTMaterialEstado.Rows.Add(
                            dr["Estado"],
                            dr["Codigo"],
                            dr["Marca"],
                            dr["Modelo"],
                            dr["TallaLongitud"],
                            dr["Categoria"],
                            dr["Stock"],
                            dr["PrecioDia"]
                        );
                    }
                }
            }

            return ds;
        }
    }
}





