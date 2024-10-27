using jocsan.Data;
using jocsan.Repository.Interfaces;
using jocsan.Repository.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar el DbContext con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorio genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar repositorios específicos y UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IAbonoRepository, AbonoRepository>();
builder.Services.AddScoped<ICreditoRepository, CreditoRepository>();
builder.Services.AddScoped<IDetalleFacturaRepository, DetalleFacturaRepository>();
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
builder.Services.AddScoped<IGasolinaRepository, GasolinaRepository>();
builder.Services.AddScoped<IMovimientosCreditoRepository, MovimientosCreditoRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
