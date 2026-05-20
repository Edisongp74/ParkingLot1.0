using Microsoft.AspNetCore.Identity;

using ParkingLot1._0.Application;
using ParkingLot1._0.Persistence;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Web.Middleware;

using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Necesario para Identity
builder.Services.AddRazorPages();

// Registro los servicios de Application (Mediator, Handlers, Validators)
builder.Services.AddApplicationServices();

// Registro los servicios de Persistence (DbContext, Repositorios)
builder.Services.AddPersistenceServices(builder.Configuration);

// Configurar Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Configuracion de contraseña
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configuracion de Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.Cookie.HttpOnly = true;

    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.SlidingExpiration = true;
});

// Configurar Session (necesario para el middleware de excepciones)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Configurar Notyf (toast notifications - como el profesor)
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Session ANTES de Authentication (como el profesor)
app.UseSession();

// Authentication debe ir antes de Authorization
app.UseAuthentication();

app.UseAuthorization();

// NECESARIO para Identity
app.MapRazorPages();

// Notyf antes del middleware de excepciones
app.UseNotyf();

// Middleware de excepciones - ULTIMO (atrapa todo)
app.UseExceptionHandlerMiddleware();

// Rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
