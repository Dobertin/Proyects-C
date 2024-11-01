using jocsan.Data;
using jocsan.Models;
using jocsan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Repository.Repositorios
{
    public class ParametroRepository : Repository<Parametro>, IParametroRepository
    {
        private readonly ApplicationDbContext _context;

        public ParametroRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ComboResultDecimal>> GetObtenerPorcentajesAsync() 
        {
            return await _context.Parametro
                        .Where(c => c.TipParametro == 1)  // Filtro primero por TipParametro
                        .Select(c => new ComboResultDecimal
                        {
                            codigo = c.ValorN,
                            descripcion = c.Descripcion
                        })
                        .ToListAsync();
        }
        public async Task<IEnumerable<ComboResultDecimal>> GetObtenerPreciohieloAsync()
        {
            return await _context.Parametro
                       .Where(c => c.TipParametro == 2)  // Filtro primero por TipParametro
                       .Select(c => new ComboResultDecimal
                       {
                           codigo = c.ValorN,
                           descripcion = c.Descripcion
                       })
                       .ToListAsync();
        }
    }
}

