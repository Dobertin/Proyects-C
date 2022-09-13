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
            Respuesta resp = new Respuesta();
            DOVenta doVenta = null;
            try
            {
                doVenta = new DOVenta();
                /*Obtenemos el Periodo en el que se huizo la venta*/
                DataTable dtFecha = doVenta.ObtenerPeriodo();
                if (dtFecha.Rows.Count>0)
                {
                    venta.periodo = dtFecha.Rows[0][0].ToString();
                }
                else
                {
                    venta.periodo = DateTime.Now.ToString("yyyy/MM");
                }
                /*Calculamos los puntos obtenidos por la venta en base al tipo de producto vendido*/
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
                /*Insertamos la venta*/
                doVenta.AgregarVenta(venta);
                /*Atualizamos la cantidad de ventas del asesor*/
                doVenta.actualizarVentaAsesor(venta.idAsesor);
                resp.resultado = true;
                resp.observacion = string.Empty;
            }
            catch (Exception ex)
            {
                resp.resultado = false;
                resp.observacion = ex.Message;                
            }
            finally
            {
                if (doVenta != null)
                {
                    doVenta.Dispose();
                    doVenta = null;
                }
            }
            return resp;
        }

        public List<VentasValidas> ListarVentas()
        {
            List<VentasValidas> resp = new List<VentasValidas>();
            VentasValidas dato = null;
            DOVenta doVenta = null;
            try
            {
                doVenta = new DOVenta();
                DataTable dtDatos = doVenta.ListarVentas();
                foreach (DataRow item in dtDatos.Rows)
                {
                    dato = new VentasValidas();
                    dato.cliente = item["cliente"].ToString();
                    dato.nombreProducto = item["nombre_com"].ToString();
                    dato.asesor = item["asesor"].ToString();
                    dato.nomtoDesembolsado = item["monto_desembolsado"].ToString();
                    dato.periodo = item["periodo"].ToString();
                    dato.puntosObtenidos = item["puntos_obtenidos"].ToString();
                    dato.fechaVenta = item["fecha_venta"].ToString();
                    resp.Add(dato);
                }
                return resp;
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

        public void CancelarVenta(int idVenta)
        {
            DOVenta doVenta = null;
            try
            {
                doVenta = new DOVenta();
                /*Obtenemos el Id del asesor que hizo al venta*/
                string idAsesor = doVenta.ObtenerIdAsesorXVenta(idVenta);
                //Devolvemos la venta
                doVenta.CancelarVenta(idVenta, idAsesor);
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
