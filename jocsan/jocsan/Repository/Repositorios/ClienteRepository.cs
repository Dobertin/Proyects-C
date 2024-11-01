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

        public async Task<IEnumerable<ComboResult>> GetComboClientesAsync()
        {
            return await _context.Cliente
                        .Select(c => new ComboResult
                        {
                            codigo = c.IdCliente,
                            descripcion = c.Nombre
                        })
                        .ToListAsync();
        }
        public async Task<IEnumerable<Cliente>> GetClientesWithAbonosAsync()
        {
            return await _context.Cliente.Include(c => c.Abonos).ToListAsync();
        }
    }
}
