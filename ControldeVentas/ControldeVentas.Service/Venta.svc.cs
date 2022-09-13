using ControldeVentas.Bussiness;
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
            BOVenta data = new BOVenta();
            data.actualizarMetaAsesor(idAsesor, cantidad);

        }
    }
}
