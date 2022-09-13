using ControldeVentas.Entity;
using ControldeVentas.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ControldeVentas.Interface
{
    [ServiceContract]
    public interface IVenta
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/AsociarMeta")]
        void AsociarMeta(int idAsesor, int cantidad);

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest,
           UriTemplate = "/AgregarVenta")]
        Respuesta AgregarVenta(Ventas venta);
    }
}
