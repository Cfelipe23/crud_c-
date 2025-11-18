using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace ClienteCRUD
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
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "INSERT INTO Producto (nombre, descripcion, precio, stock) VALUES (@nombre, @descripcion, @precio, @stock)";

      cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
      cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@precio", producto.Precio);
      cmd.Parameters.AddWithValue("@stock", producto.Stock);

      await cmd.ExecuteNonQueryAsync();

      cmd.CommandText = "SELECT last_insert_rowid()";
      producto.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

      return producto;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
      var productos = new List<Producto>();
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT * FROM Producto";

      using var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        productos.Add(new Producto
        {
          Id = reader.GetInt32(0),
          Nombre = reader.GetString(1),
          Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
          Precio = reader.GetDecimal(3),
          Stock = reader.GetInt32(4)
        });
      }

      return productos;
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT * FROM Producto WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);

      using var reader = await cmd.ExecuteReaderAsync();
      if (await reader.ReadAsync())
      {
        return new Producto
        {
          Id = reader.GetInt32(0),
          Nombre = reader.GetString(1),
          Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
          Precio = reader.GetDecimal(3),
          Stock = reader.GetInt32(4)
        };
      }

      return null;
    }

    public async Task<bool> UpdateAsync(Producto producto)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "UPDATE Producto SET nombre = @nombre, descripcion = @descripcion, precio = @precio, stock = @stock WHERE id = @id";

      cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
      cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@precio", producto.Precio);
      cmd.Parameters.AddWithValue("@stock", producto.Stock);
      cmd.Parameters.AddWithValue("@id", producto.Id);

      var rowsAffected = await cmd.ExecuteNonQueryAsync();
      return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "DELETE FROM Producto WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);

      var rowsAffected = await cmd.ExecuteNonQueryAsync();
      return rowsAffected > 0;
    }

  }
}
