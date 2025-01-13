using jocsan.Data;
using jocsan.Models;
using jocsan.Models.results;
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
        public async Task<decimal> ObtenerGasolinaPorID(int idCliente)
        {
            // Obtener el valor de Gasolina para el cliente especificado
            var gasolina = await _context.Cliente
                .Where(c => c.IdCliente == idCliente)
                .Select(c => c.Gasolina)
                .FirstOrDefaultAsync();

            return gasolina;
        }
        public async Task<IEnumerable<ComboResult>> GetComboClientesAsync()
        {
            return await _context.Cliente
                        .Select(c => new ComboResult
                        {
                            codigo = c.IdCliente,
                            descripcion = c.Capitan
                        })
                        .ToListAsync();
        }
        public async Task<IEnumerable<Cliente>> GetClientesWithAbonosAsync()
        {
            return await _context.Cliente.Include(c => c.Abonos).ToListAsync();
        }
        public async Task<IEnumerable<ClienteResult>> ObtenerInformacionClientesAsync()
        {
            return await _context.Cliente
                        .Where(c => c.Estado == 1)
                        .Select(c => new ClienteResult
                        {
                            IdCliente = c.IdCliente,
                            Nombre = c.Nombre,
                            Capitan = c.Capitan,
                            Porcentaje = c.Porcentaje.ToString() + "%",
                            Gasolina = c.Gasolina.ToString()
                        })
                        .ToListAsync();
        }
        public async Task EliminarClienteAsync(int idcliente)
        {
            // Obtener la factura por su Id
            var cliente = await _context.Cliente.FindAsync(idcliente);

            if (cliente == null)
            {
                throw new Exception("Cliente no encontrado.");
            }

            // Modificar los valores de la entidad
            cliente.Estado = 0;
            cliente.FechaModifica = DateTime.Now;
            cliente.UsuarioModifica = Environment.UserName; // Asigna el usuario actual del sistema

            // Marcar la entidad como modificada y guardar cambios
            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
