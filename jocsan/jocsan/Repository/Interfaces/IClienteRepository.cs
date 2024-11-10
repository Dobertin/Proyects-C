using jocsan.Models;
using jocsan.Models.results;

namespace jocsan.Repository.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task EliminarClienteAsync(int idcliente);

        // Métodos adicionales específicos de Cliente
        Task<IEnumerable<Cliente>> GetClientesWithAbonosAsync();
        Task<IEnumerable<ComboResult>> GetComboClientesAsync();
        Task<decimal> ObtenerGasolinaPorID(int idCliente);
        Task<IEnumerable<ClienteResult>> ObtenerInformacionClientesAsync();
    }
}
