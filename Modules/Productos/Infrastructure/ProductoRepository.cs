using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;
using crud_c_.Modules.Productos.Application;
using crud_c_.Modules.Productos.Domain;
using crud_c_.Infrastructure.Persistence;
using Dapper;

namespace crud_c_.Modules.Productos.Infrastructure
{
  public class ProductoRepository : IProductoRepository
  {
    private readonly SqliteConnectionFactory _factory;

    public ProductoRepository(SqliteConnectionFactory factory)
    {
      _factory = factory;
    }

    public async Task<Producto> AddAsync(Producto producto)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          INSERT INTO Producto (nombre, descripcion, precio, stock) 
          VALUES (@Nombre, @Descripcion, @Precio, @Stock);
          SELECT last_insert_rowid();";

        producto.Id = await conn.QuerySingleAsync<int>(sql, producto);
        return producto;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al agregar producto {producto.Nombre}", ex);
      }
    }

    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Producto ORDER BY Nombre";
        return await conn.QueryAsync<Producto>(sql);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Error al obtener todos los productos", ex);
      }
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Producto WHERE Id = @id";
        return await conn.QueryFirstOrDefaultAsync<Producto>(sql, new { id });
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al obtener producto con ID {id}", ex);
      }
    }

    public async Task<bool> UpdateAsync(Producto producto)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          UPDATE Producto 
          SET nombre = @Nombre, descripcion = @Descripcion, 
              precio = @Precio, stock = @Stock 
          WHERE id = @Id";

        var rowsAffected = await conn.ExecuteAsync(sql, producto);
        return rowsAffected > 0;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al actualizar producto con ID {producto.Id}", ex);
      }
    }

    public async Task<bool> DeleteAsync(int id)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "DELETE FROM Producto WHERE id = @id";
        var rowsAffected = await conn.ExecuteAsync(sql, new { id });
        return rowsAffected > 0;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al eliminar producto con ID {id}", ex);
      }
    }
  }
}