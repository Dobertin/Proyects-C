using Microsoft.EntityFrameworkCore;
using Sistema.Database.Context;
using Sistema.Database.Interfaces;
using Sistema.Database.Repository;
using Sistema.Negocio.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MiContexto>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySQLConnection")));

// Registrar repositorio genérico
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Registrar repositorio UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registrar Services
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<TiendaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

app.Run();
