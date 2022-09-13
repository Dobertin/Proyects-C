using ControldeVentas.DataAccess;
using ControldeVentas.Entity.DTO;
using ControldeVentas.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ControldeVentas.Bussiness
{
    public class BOVenta
    {
        public void actualizarMetaAsesor(int idAsesor, int cantidad)
        {
            DOVenta doVenta = null;
            try
            {
                doVenta = new DOVenta();
                doVenta.actualizarMetaAsesor(idAsesor, cantidad);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (doVenta!= null)
                {
                    doVenta.Dispose();
                    doVenta = null;
                }
            }
        }

        public Respuesta AgregarVenta(Ventas venta)
        {
            DOVenta doVenta = null;
            try
            {
                doVenta = new DOVenta();
                /*Calculamos el*/
                string tipoProducto = string.Empty;
                decimal puntos = 0;
                DataTable dataProducto = doVenta.ObtenerTipoProducto(venta.idProducto);
                foreach (DataRow dr in dataProducto.Rows)
                {
                    tipoProducto = dr["tipo"].ToString().Trim();
                    puntos = Convert.ToDecimal(dr["puntos"]);
                }
                if (tipoProducto.Equals(Constantes.PRODUCTOTARJETA))
                {
                    venta.puntosObtenidos = puntos;
                }
                else if (tipoProducto.Equals(Constantes.PRODUCTOCREDITO))
                {
                    venta.puntosObtenidos = venta.montoDesembolsado * puntos;
                }
                //doVenta.actualizarMetaAsesor(venta);
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (doVenta != null)
                {
                    doVenta.Dispose();
                    doVenta = null;
                }
            }
        }
    }
}
