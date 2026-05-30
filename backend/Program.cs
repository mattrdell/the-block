using Microsoft.EntityFrameworkCore;
using TheBlock.Api.Data;
using TheBlock.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VehiclesContext>(options =>
    options.UseInMemoryDatabase("Vehicles"));

builder.Services.AddScoped<IVehiclesService, VehiclesService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VehiclesContext>();
    var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    await VehicleDataSeeder.SeedAsync(context, environment);
}

app.Run();

public partial class Program;

