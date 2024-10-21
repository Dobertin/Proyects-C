using jocsan.Models;

namespace jocsan.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IAbonoRepository Abono { get; }
        ICreditoRepository Credito { get; }
        IDetalleFacturaRepository DetalleFactura { get; }
        IFacturaRepository Factura { get; }
        IMovimientosCreditoRepository MovimientosCredito { get; }
        IProductoRepository Producto { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}
