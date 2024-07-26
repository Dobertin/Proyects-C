using Facturacion.Data;
using Facturacion.Repositories;
using Microsoft.Extensions.Options;
using Facturacion.Models;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<ListaPrecioRepository>();
builder.Services.AddScoped<PromocionRepository>();
builder.Services.AddScoped<TipoImpuestoRepository>();
builder.Services.AddScoped<TipoPagoRepository>();
builder.Services.AddScoped<FacturaRepository>();

// Add authentication and authorization services
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<RolRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/Account/Login";
        config.AccessDeniedPath = "/Account/AccessDenied";
        config.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireRole("Admin"));
    config.AddPolicy("User", policyBuilder => policyBuilder.RequireRole("User"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Mostrar detalles de las excepciones en el entorno de desarrollo
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Add this line to enable authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Configure Rotativa
RotativaConfiguration.Setup(app.Environment.WebRootPath);

app.Run();
