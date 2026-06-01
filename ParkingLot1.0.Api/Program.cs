using Microsoft.AspNetCore.Identity;
using ParkingLot1._0.Application;
using ParkingLot1._0.Persistence;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Persistence.Identity;
using ParkingLot1._0.Persistence.Seeding;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<SeedDb>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    SeedDb service = scope.ServiceProvider.GetRequiredService<SeedDb>();
    await service.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
