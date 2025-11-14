using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Profiles;
using Domain.Interfaces;
using Application.Services;
using System.Text.Json.Serialization;           // <-- importante
using Microsoft.OpenApi.Models;                 // <-- importante
using Microsoft.AspNetCore.Builder; // Importa el espacio de nombres necesario para construir y configurar la aplicación web.
using Microsoft.Extensions.DependencyInjection; // Importa el espacio de nombres necesario para configurar los servicios de la aplicación.
using Microsoft.Extensions.Hosting; // Importa el espacio de nombres necesario para trabajar con diferentes entornos (desarrollo, producción, etc.).
using app.backend.Services; // Importa el espacio de nombres donde se encuentran los servicios personalizados de la aplicación.
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
	
builder.Services.AddScoped<IProductoService, ProductoServiceEf>();

// Controllers + JSON (evita problemas de ciclos y referencias)
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger (registra el doc "v1")
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Taller Aur API",
        Version = "v1",
        Description = "API del Taller con EF Core + DTOs + Ventas"
    });
	c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string", Format = "date"
    });
    c.MapType<TimeOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string", Format = "time"
    });

    // evita choques de nombres si hay tipos con el mismo nombre en distintos namespaces
    c.CustomSchemaIds(type => type.FullName);
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:5173", "http://localhost:4200")
              .AllowCredentials());
});

// DI de servicios
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IProductoService, ProductoServiceEf>();
builder.Services.AddScoped<IProveedorService, ProveedorServiceEf>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taller Aur API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();