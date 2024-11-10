using jocsan.Models;
using jocsan.Models.results;

namespace jocsan.Repository.Interfaces
{
    public interface ICreditoRepository : IRepository<Credito>
    {
        Task EliminarCreditoAsync(int idcredito);

        Task<IEnumerable<CreditoResults>> ObtenerCreditosPorClienteAsync(int idcliente);
        Task<CreditoResultsExtended> ObtenerCreditosPorClienteTotalAsync(int idcliente);
    }
}