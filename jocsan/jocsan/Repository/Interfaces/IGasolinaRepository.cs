using jocsan.Models;
using jocsan.Models.results;

namespace jocsan.Repository.Interfaces
{
    public interface IGasolinaRepository : IRepository<Gasolina>
    {
        Task<GasolinaResult> ObtenerDatosGasolinaPorID(int idCliente);
    }
}
