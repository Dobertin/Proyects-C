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
        public DbSet<Abono> Abono { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Credito> Creditos { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<DetalleFactura> DetalleFactura { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<MovimientosCredito> MovimientosCredito { get; set; }
        public DbSet<Gasolina> Gasolina { get; set; }
        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<Vuelto> Vuelto { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Aquí puedes agregar configuraciones adicionales, si las necesitas
        }
    }

}
