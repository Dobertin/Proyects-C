using jocsan.Models;
using jocsan.Models.results;

namespace jocsan.Repository.Interfaces
{
    public interface IAbonoRepository : IRepository<Abono>
    {
        Task<AbonoResultExtended> ObtenerAbonoPorClienteTotalAsync(int idcliente);
        Task<CuentaResult> ObtenerUltimoAbonoPorClienteAsync(int idcliente);
    }
}
