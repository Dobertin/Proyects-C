using jocsan.Models;
using jocsan.Models.querys;
using jocsan.Models.results;

namespace jocsan.Repository.Interfaces
{
    public interface IFacturaRepository : IRepository<Factura>
    {
        Task<IEnumerable<FacturaResults>> ObtenerFacturasAsync(FacturaQuery facturaQuery);
        Task<int> ObtenerUltimoNumeroFacturaAsync();
        Task EliminarFacturaAsync(int idFactura);
        Task<FacturaResultsExtended> ObtenerFacturasPorClienteAsync(int idcliente);
    }
}
