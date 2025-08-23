using TennisCleanArchi.Infrastructure;
using TennisCleanArchi.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseInfrastructure();

app.UseAuthorization();

app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    await seeder.SeedAsync();
}

app.Run();
