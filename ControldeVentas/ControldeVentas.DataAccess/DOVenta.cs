using ControldeVentas.Entity;
using ControldeVentas.Entity.DTO;
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

        public DataTable ObtenerPeriodo()
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(" SELECT (CONVERT(varchar(4),year(GETDATE()))+'/'+ CONVERT(varchar(4),MONTH(GetDate()))) as Fecha ");
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            SqlDataReader reader = comando.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
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

        public void actualizarVentaAsesor(int idAsesor)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append("update asesores set cant_ventas = cant_ventas + 1 where id_asesor = " + idAsesor);
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            comando.ExecuteNonQuery();
        }

        public void AgregarVenta(Ventas venta)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append("insert into ventas ");
            sSQL.Append("values ( ");
            sSQL.Append(venta.idCliente == 0 ? "0, " : "" + venta.idCliente + ",");
            sSQL.Append(venta.idAsesor == 0 ? "0, " : "" + venta.idAsesor + ",");
            sSQL.Append(venta.idProducto == 0 ? "0, " : "" + venta.idProducto + ",");
            sSQL.Append(string.IsNullOrWhiteSpace(venta.periodo) ? "null, " : "'" + venta.periodo + "',");
            sSQL.Append(venta.puntosObtenidos == 0 ? "0, " : "" + venta.puntosObtenidos.ToString().Replace(',','.') + ",");
            sSQL.Append("getdate(),");
            sSQL.Append(venta.montoDesembolsado == 0 ? "0,1 " : "" + venta.montoDesembolsado.ToString().Replace(',', '.') + ", 1");
            sSQL.Append(");");
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            comando.ExecuteNonQuery();
        }

        public DataTable ListarVentas()
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append(" select c.nombres cliente, p.nombre_com,a.usuario asesor,v.monto_desembolsado, ");
            sSQL.Append(" v.periodo, v.puntos_obtenidos, v.fecha_venta ");
            sSQL.Append(" from ventas v inner join cliente c on v.id_Cliente = c.id_cliente ");
            sSQL.Append(" inner join asesores a on v.id_asesor = a.id_asesor ");
            sSQL.Append(" inner join productos p on v.id_producto = p.id_producto where estado_registro =1 ");
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            SqlDataReader reader = comando.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }

        public void CancelarVenta(int idVenta, string idAsesor)
        {
            SqlCommand comando = null;
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append("update ventas set estado_registro = 3 where id_venta = " + idVenta);
            comando = new SqlCommand(sSQL.ToString(), conexion);
            comando.ExecuteNonQuery();

            sSQL = new StringBuilder();
            sSQL.Append("update asesores set cant_ventas = cant_ventas - 1 where id_asesor = " + idAsesor);
            comando = new SqlCommand(sSQL.ToString(), conexion);
            comando.ExecuteNonQuery();
        }

        public string ObtenerIdAsesorXVenta(int idVenta)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.Append("select id_asesor from ventas where id_venta = " + idVenta);
            SqlCommand comando = new SqlCommand(sSQL.ToString(), conexion);
            return comando.ExecuteScalar().ToString();
        }
    }
}
