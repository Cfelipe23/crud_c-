using crud_c_.Infrastructure.Persistence;
using crud_c_.Modules.Clientes.Application;
using crud_c_.Modules.Clientes.Infrastructure;
using crud_c_.Modules.Lotes.Application;
using crud_c_.Modules.Lotes.Infrastructure;
using crud_c_.Modules.Productos.Application;
using crud_c_.Modules.Productos.Infrastructure;
using Dapper;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.PropertyNamingPolicy = null;
      options.JsonSerializerOptions.WriteIndented = true;
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Sistema CRUD API",
    Version = "v1",
    Description = "API para gestión de Clientes, Lotes y Productos"
  });
  // Incluir comentarios XML (generados por el proyecto) para que aparezcan en Swagger
  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  if (File.Exists(xmlPath))
  {
    c.IncludeXmlComments(xmlPath);
  }
  // Habilitar filtros de ejemplos
  c.ExampleFilters();
});

// Registrar los providers de ejemplos
builder.Services.AddSwaggerExamplesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// Configurar CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      builder =>
      {
        builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
      });
});

// Configurar servicios
string dbPath = Path.Combine(builder.Environment.ContentRootPath, "clientes.db");
builder.Services.AddSingleton<SqliteConnectionFactory>(sp =>
    new SqliteConnectionFactory($"Data Source={dbPath}"));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ILoteRepository, LoteRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema CRUD API V1");
    c.RoutePrefix = "swagger";
  });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Asegurar que la base de datos existe
using (var scope = app.Services.CreateScope())
{
  try
  {
    var factory = scope.ServiceProvider.GetRequiredService<SqliteConnectionFactory>();
    using var connection = factory.CreateConnection();
    connection.Open();

    // Crear tablas si no existen
    connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Clientes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Apellido TEXT NOT NULL,
                Direccion TEXT,
                Telefono TEXT,
                Email TEXT
            );
            
            CREATE TABLE IF NOT EXISTS Productos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Descripcion TEXT,
                Precio DECIMAL(10,2) NOT NULL,
                Stock INTEGER NOT NULL
            );
            
            CREATE TABLE IF NOT EXISTS Lotes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Codigo TEXT NOT NULL,
                FechaIngreso DATE NOT NULL,
                Cantidad INTEGER NOT NULL,
                ProductoId INTEGER NOT NULL,
                FOREIGN KEY(ProductoId) REFERENCES Productos(Id)
            );
        ");

    app.Logger.LogInformation("Base de datos inicializada correctamente.");
  }
  catch (Exception ex)
  {
    app.Logger.LogError(ex, "Error al inicializar la base de datos.");
    throw;
  }
}

app.Run();
