using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;
using ClienteCRUD.Modules.Lotes.Application;
using ClienteCRUD.Modules.Lotes.Domain;
using ClienteCRUD.Infrastructure.Persistence;
using Dapper;

namespace ClienteCRUD.Modules.Lotes.Infrastructure
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
          INSERT INTO Lote (codigo, fecha_ingreso, cantidad, producto_id) 
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
            fecha_ingreso as FechaIngreso, 
            Cantidad, 
            producto_id as ProductoId 
          FROM Lote 
          ORDER BY fecha_ingreso DESC";

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
            fecha_ingreso as FechaIngreso, 
            Cantidad, 
            producto_id as ProductoId 
          FROM Lote 
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
          UPDATE Lote 
          SET codigo = @Codigo, fecha_ingreso = @FechaIngreso, 
              cantidad = @Cantidad, producto_id = @ProductoId 
          WHERE id = @Id";

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
        const string sql = "DELETE FROM Lote WHERE id = @id";
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