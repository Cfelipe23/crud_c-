using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;
using crud_c_.Modules.Lotes.Application;
using crud_c_.Modules.Lotes.Domain;
using crud_c_.Infrastructure.Persistence;
using Dapper;

namespace crud_c_.Modules.Lotes.Infrastructure
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
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          INSERT INTO Lotes (Codigo, FechaIngreso, Cantidad, ProductoId) 
          VALUES (@Codigo, @FechaIngreso, @Cantidad, @ProductoId);
          SELECT last_insert_rowid();";

        var parameters = new
        {
          lote.Codigo,
          FechaIngreso = lote.FechaIngreso.ToString("yyyy-MM-dd"),
          lote.Cantidad,
          lote.ProductoId
        };

        lote.Id = await conn.QuerySingleAsync<int>(sql, parameters);
        return lote;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al agregar lote {lote.Codigo}", ex);
      }
    }

    public async Task<IEnumerable<Lote>> GetAllAsync()
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          SELECT 
            Id, 
            Codigo, 
            FechaIngreso, 
            Cantidad, 
            ProductoId 
          FROM Lotes 
          ORDER BY FechaIngreso DESC";

        return await conn.QueryAsync<Lote>(sql);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Error al obtener todos los lotes", ex);
      }
    }

    public async Task<Lote?> GetByIdAsync(int id)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          SELECT 
            Id, 
            Codigo, 
            FechaIngreso, 
            Cantidad, 
            ProductoId 
          FROM Lotes 
          WHERE Id = @id";

        return await conn.QueryFirstOrDefaultAsync<Lote>(sql, new { id });
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al obtener lote con ID {id}", ex);
      }
    }

    public async Task<bool> UpdateAsync(Lote lote)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          UPDATE Lotes 
          SET Codigo = @Codigo, FechaIngreso = @FechaIngreso, 
              Cantidad = @Cantidad, ProductoId = @ProductoId 
          WHERE Id = @Id";

        var parameters = new
        {
          lote.Codigo,
          FechaIngreso = lote.FechaIngreso.ToString("yyyy-MM-dd"),
          lote.Cantidad,
          lote.ProductoId,
          lote.Id
        };

        var rowsAffected = await conn.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al actualizar lote con ID {lote.Id}", ex);
      }
    }

    public async Task<bool> DeleteAsync(int id)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "DELETE FROM Lotes WHERE Id = @id";
        var rowsAffected = await conn.ExecuteAsync(sql, new { id });
        return rowsAffected > 0;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al eliminar lote con ID {id}", ex);
      }
    }
  }
}