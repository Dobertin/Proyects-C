using ControldeVentas.Bussiness;
using ControldeVentas.Entity;
using ControldeVentas.Entity.DTO;
using ControldeVentas.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ControldeVentas.Service
{
    public class Venta : IVenta
    {
        public void AsociarMeta(int idAsesor, int cantidad)
        {
            try
            {
                BOVenta data = new BOVenta();
                data.actualizarMetaAsesor(idAsesor, cantidad); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Respuesta AgregarVenta(Ventas venta)
        {
            try
            {
                BOVenta data = new BOVenta();
                return data.AgregarVenta(venta);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void CancelarVenta(int idVenta)
        {
            try
            {
                BOVenta data = new BOVenta();
                data.CancelarVenta(idVenta);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<VentasValidas> ListarVentas()
        {
            try
            {
                BOVenta data = new BOVenta();
                return data.ListarVentas();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
