using jocsan.Models;

namespace jocsan.Repository.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        // Métodos adicionales específicos de Cliente
        Task<IEnumerable<Cliente>> GetClientesWithAbonosAsync();
    }
}
