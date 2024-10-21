using jocsan.Models;
using Microsoft.EntityFrameworkCore;

namespace jocsan.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Abono> Abonos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Credito> Creditos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<MovimientosCredito> movimientosCreditos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Aquí puedes agregar configuraciones adicionales, si las necesitas
        }
    }

}
