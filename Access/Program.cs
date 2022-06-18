using Access.Data;
using Access.Data.Repository;
using Access.Interfaces.Repository;
using Access.Interfaces.Services;
using Access.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();

// Swagger Config
services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Entity Framework Core
var ConnectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<Context>(options =>
                options.UseSqlServer(ConnectionString));

// Http Context Accessor
services.AddHttpContextAccessor();

// Services
services.AddScoped<IUserServices, UserServices>();
services.AddScoped<IAccessLoggerServices, AccessLoggerServices>();
services.AddScoped<IAccessServices, AccessServices>();

// Repositories
services.AddHttpContextAccessor();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAccessLoggerRepository, AccessLoggerRepository>();

// JWT
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(option =>
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = configuration["TokenConfiguration:Audience"],
        ValidIssuer = configuration["TokenConfiguration:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
    });

// Set main roles
var admin = AccessServices.MainPermissions["Admin"];
var common = AccessServices.MainPermissions["Common"];
services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",  policy => policy.RequireClaim("Admin",  admin));
    options.AddPolicy("Common", policy => policy.RequireClaim("Common", admin, common));
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Enable Cors
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.MapControllers();

app.Run();
