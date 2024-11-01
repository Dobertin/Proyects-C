using jocsan.Models;

namespace jocsan.Repository.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<ComboResult>> GetComboProductosAsync();
    }
}
