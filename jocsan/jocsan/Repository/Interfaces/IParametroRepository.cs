using jocsan.Models;

namespace jocsan.Repository.Interfaces
{
    public interface IParametroRepository : IRepository<Parametro>
    {
        Task<IEnumerable<ComboResultDecimal>> GetObtenerPorcentajesAsync();
        Task<IEnumerable<ComboResultDecimal>> GetObtenerPreciohieloAsync();
        Task<IEnumerable<ComboResultDecimal>> GetObtenerPrecioGasolinaAsync();        
    }
}
