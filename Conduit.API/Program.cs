using Conduit.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var Services = builder.Services;

// Add services to the container.

Services.AddControllers();

Services.AddServices();

Services.AddDbContext(builder);

Services.AddAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
