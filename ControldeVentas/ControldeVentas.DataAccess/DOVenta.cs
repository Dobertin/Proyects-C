using ControldeVentas.Entity;
using System;
using System.Collections.Generic;
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
            SqlConnection conexion = new SqlConnection(Constantes.STRINGCONECTION);
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
        #endregion

        public void actualizarMetaAsesor(int idAsesor, int cantidad)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append("Update asesores set meta_propuesta = " + cantidad);
            sSQL.Append(" where idAsesor =" + idAsesor);
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            comando.ExecuteNonQuery();
        }
    }
}
