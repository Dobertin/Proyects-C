using ControldeVentas.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.DataAccess
{
    public class DOVenta
    {
        private bool _disposed = false;
        private SqlConnection conexion = null;
                
        #region privados
        public DOVenta()
        {
            conexion = new SqlConnection(Constantes.STRINGCONECTION);
            conexion.Open();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                        conexion.Dispose();
                        conexion = null;
                    }
                }
            }
            _disposed = true;
        }

        public DataTable ObtenerTipoProducto(int idProducto)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(" select tipo, puntos from productos where id_producto = " + idProducto);
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            SqlDataReader reader = comando.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;

        }
        #endregion

        public void actualizarMetaAsesor(int idAsesor, int cantidad)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append("Update asesores set meta_propuesta = " + cantidad);
            sSQL.Append(" where id_asesor =" + idAsesor);
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            comando.ExecuteNonQuery();
        }
    }
}
