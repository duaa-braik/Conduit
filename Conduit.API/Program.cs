var builder = WebApplication.CreateBuilder(args);

var Services = builder.Services;

// Add services to the container.

Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
