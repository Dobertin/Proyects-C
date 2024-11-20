using jocsan.Models;
using jocsan.Models.results;

namespace jocsan.Repository.Interfaces
{
    public interface IVueltoRepository : IRepository<Vuelto>
    {
        Task<VueltoResultExtended> ObtenerVueltoPorClienteTotalAsync(int idcliente); 
    }
}
