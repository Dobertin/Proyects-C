using Sistema.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Database.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Rol> Roles { get; }
        IGenericRepository<Tienda> Tiendas { get; }
        IGenericRepository<Usuario> Usuarios { get; }
        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

}
