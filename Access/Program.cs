using Access.Data;
using Access.Data.Repository;
using Access.Interfaces.Repository;
using Access.Interfaces.Services;
using Access.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var ConnectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<Context>(options =>
                options.UseSqlServer(ConnectionString));

// Services
services.AddScoped<IUserServices, UserServices>();
services.AddScoped<IAccessLoggerServices, AccessLoggerServices>();

// Repositories
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAccessLoggerRepository, AccessLoggerRepository>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
