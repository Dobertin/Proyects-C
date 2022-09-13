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

        public void actualizarMetaAsesor(int idAsesor, int cantidad)
        {
            throw new NotImplementedException();
        }
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
    }
}
