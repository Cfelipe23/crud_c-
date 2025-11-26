using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ClienteCRUD.Modules.Clientes.Application;
using ClienteCRUD.Modules.Clientes.Infrastructure;
using ClienteCRUD.Modules.Lotes.Application;
using ClienteCRUD.Modules.Lotes.Infrastructure;
using ClienteCRUD.Modules.Productos.Application;
using ClienteCRUD.Modules.Productos.Infrastructure;

namespace ClienteCRUD
{
  class Program
  {
    /// <summary>
    /// Punto de entrada principal de la aplicación. Configura la API REST.
    /// </summary>
    static async Task Main(string[] args)
    {
      const string connString = "DataSource=clientes.db";
      var factory = new SqliteConnectionFactory(connString);

      // Crear builder de la aplicación
      var builder = WebApplication.CreateBuilder(args);

      // Agregar servicios
      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowAll", policy =>
        {
          policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
      });

      builder.Services.AddControllers();
      
      // Registrar repositorios como servicios
      builder.Services.AddSingleton(_ => factory);
      builder.Services.AddScoped<IClienteRepository>(_ => new ClienteRepository(factory));
      builder.Services.AddScoped<IProductoRepository>(_ => new ProductoRepository(factory));
      builder.Services.AddScoped<ILoteRepository>(_ => new LoteRepository(factory));

      var app = builder.Build();

      // Middleware
      app.UseCors("AllowAll");
      app.UseRouting();
      app.MapControllers();

      await app.RunAsync();
    }
  }
}
