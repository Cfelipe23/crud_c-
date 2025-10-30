using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;
using crud_c_.Modules.Clientes.Application;
using crud_c_.Modules.Clientes.Domain;
using crud_c_.Infrastructure.Persistence;
using Dapper;

namespace crud_c_.Modules.Clientes.Infrastructure
{
  public class ClienteRepository : IClienteRepository
  {
    private readonly SqliteConnectionFactory _factory;

    public ClienteRepository(SqliteConnectionFactory factory)
    {
      _factory = factory;
    }

    public async Task<Cliente> AddAsync(Cliente cliente)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          INSERT INTO Cliente (nombre, apellido, direccion, telefono, email) 
          VALUES (@Nombre, @Apellido, @Direccion, @Telefono, @Email);
          SELECT last_insert_rowid();";

        cliente.Id = await conn.QuerySingleAsync<int>(sql, cliente);
        return cliente;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al agregar cliente {cliente.Nombre}", ex);
      }
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "SELECT Id, Nombre, Apellido, Direccion, Telefono, Email FROM Cliente ORDER BY Id ASC";
        return await conn.QueryAsync<Cliente>(sql);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Error al obtener todos los clientes", ex);
      }
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "SELECT Id, Nombre, Apellido, Direccion, Telefono, Email FROM Cliente WHERE Id = @id";
        return await conn.QueryFirstOrDefaultAsync<Cliente>(sql, new { id });
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al obtener cliente con ID {id}", ex);
      }
    }

    public async Task<bool> UpdateAsync(Cliente cliente)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = @"
          UPDATE Cliente 
          SET nombre = @Nombre, apellido = @Apellido, direccion = @Direccion, 
              telefono = @Telefono, email = @Email 
          WHERE id = @Id";

        var rowsAffected = await conn.ExecuteAsync(sql, cliente);
        return rowsAffected > 0;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al actualizar cliente con ID {cliente.Id}", ex);
      }
    }

    public async Task<bool> DeleteAsync(int id)
    {
      try
      {
        using var conn = _factory.CreateConnection();
        const string sql = "DELETE FROM Cliente WHERE id = @id";
        var rowsAffected = await conn.ExecuteAsync(sql, new { id });
        return rowsAffected > 0;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Error al eliminar cliente con ID {id}", ex);
      }
    }
  }
}