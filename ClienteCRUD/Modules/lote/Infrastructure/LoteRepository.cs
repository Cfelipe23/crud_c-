using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace ClienteCRUD
{
  public class LoteRepository : ILoteRepository
  {
    private readonly SqliteConnectionFactory _factory;

    public LoteRepository(SqliteConnectionFactory factory)
    {
      _factory = factory;
    }

    public async Task<Lote> AddAsync(Lote lote)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "INSERT INTO Lote (codigo, fecha_ingreso, cantidad, producto_id) VALUES (@codigo, @fecha, @cantidad, @productoId)";

      cmd.Parameters.AddWithValue("@codigo", lote.Codigo);
      cmd.Parameters.AddWithValue("@fecha", lote.FechaIngreso.ToString("yyyy-MM-dd"));
      cmd.Parameters.AddWithValue("@cantidad", lote.Cantidad);
      cmd.Parameters.AddWithValue("@productoId", lote.ProductoId);

      await cmd.ExecuteNonQueryAsync();

      cmd.CommandText = "SELECT last_insert_rowid()";
      lote.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

      return lote;
    }

    public async Task<IEnumerable<Lote>> GetAllAsync()
    {
      var lotes = new List<Lote>();
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT * FROM Lote";

      using var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        lotes.Add(new Lote
        {
          Id = reader.GetInt32(0),
          Codigo = reader.GetString(1),
          FechaIngreso = DateTime.Parse(reader.GetString(2)),
          Cantidad = reader.GetInt32(3),
          ProductoId = reader.GetInt32(4)
        });
      }

      return lotes;
    }

    public async Task<Lote?> GetByIdAsync(int id)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT * FROM Lote WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);

      using var reader = await cmd.ExecuteReaderAsync();
      if (await reader.ReadAsync())
      {
        return new Lote
        {
          Id = reader.GetInt32(0),
          Codigo = reader.GetString(1),
          FechaIngreso = DateTime.Parse(reader.GetString(2)),
          Cantidad = reader.GetInt32(3),
          ProductoId = reader.GetInt32(4)
        };
      }

      return null;
    }

    public async Task<bool> UpdateAsync(Lote lote)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "UPDATE Lote SET codigo = @codigo, fecha_ingreso = @fecha, cantidad = @cantidad, producto_id = @productoId WHERE id = @id";

      cmd.Parameters.AddWithValue("@codigo", lote.Codigo);
      cmd.Parameters.AddWithValue("@fecha", lote.FechaIngreso.ToString("yyyy-MM-dd"));
      cmd.Parameters.AddWithValue("@cantidad", lote.Cantidad);
      cmd.Parameters.AddWithValue("@productoId", lote.ProductoId);
      cmd.Parameters.AddWithValue("@id", lote.Id);

      var rowsAffected = await cmd.ExecuteNonQueryAsync();
      return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "DELETE FROM Lote WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);

      var rowsAffected = await cmd.ExecuteNonQueryAsync();
      return rowsAffected > 0;
    }


  }
}
