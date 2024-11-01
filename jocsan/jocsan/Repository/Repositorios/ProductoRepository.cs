using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Repository.Repositorios
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ComboResult>> GetComboProductosAsync() 
        {
            return await _context.Producto
                        .Select(c => new ComboResult
                        {
                            codigo = c.IdProducto,
                            descripcion = c.Codigo.Trim()
                        })
                        .ToListAsync();
        }
    }
}
