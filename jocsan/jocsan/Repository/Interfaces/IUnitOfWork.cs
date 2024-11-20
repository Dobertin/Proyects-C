using jocsan.Models;

namespace jocsan.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Cliente { get; }
        IAbonoRepository Abono { get; }
        ICreditoRepository Creditos { get; }
        IDetalleFacturaRepository DetalleFactura { get; }
        IFacturaRepository Factura { get; }
        IMovimientosCreditoRepository MovimientosCredito { get; }
        IProductoRepository Producto { get; }
        IGasolinaRepository Gasolina { get; }
        IParametroRepository Parametro { get; }
        IVueltoRepository Vuelto { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
        Task<int> SaveChangesAsync();
    }
}
