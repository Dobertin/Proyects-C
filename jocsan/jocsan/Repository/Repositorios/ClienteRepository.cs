using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Repository.Repositorios
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetClientesWithAbonosAsync()
        {
            return await _context.Clientes.Include(c => c.Abonos).ToListAsync();
        }
    }
}
