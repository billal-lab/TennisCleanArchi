using TennisCleanArchi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseInfrastructure();

app.UseAuthorization();

app.MapControllers();

app.Run();
