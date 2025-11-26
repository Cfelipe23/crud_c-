using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Linq;
using ClienteCRUD.Modules.Clientes.Domain;
using ClienteCRUD.Modules.Clientes.Application;

namespace ClienteCRUD.Modules.Clientes.Infrastructure
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
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "INSERT INTO Cliente (nombre, apellido, direccion, telefono, email) VALUES (@nombre, @apellido, @direccion, @telefono, @email)";

      cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
      cmd.Parameters.AddWithValue("@apellido", cliente.Apellido);
      cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);
      cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
      cmd.Parameters.AddWithValue("@email", cliente.Email);

      await cmd.ExecuteNonQueryAsync();

      cmd.CommandText = "SELECT last_insert_rowid()";
      cliente.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

      return cliente;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
      var clientes = new List<Cliente>();
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT * FROM Cliente";

      using var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        clientes.Add(new Cliente
        {
          Id = reader.GetInt32(0),
          Nombre = reader.GetString(1),
          Apellido = reader.GetString(2),
          Direccion = reader.IsDBNull(3) ? null : reader.GetString(3),
          Telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
          Email = reader.IsDBNull(5) ? null : reader.GetString(5)
        });
      }

      return clientes;
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "SELECT * FROM Cliente WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);

      using var reader = await cmd.ExecuteReaderAsync();
      if (await reader.ReadAsync())
      {
        return new Cliente
        {
          Id = reader.GetInt32(0),
          Nombre = reader.GetString(1),
          Apellido = reader.GetString(2),
          Direccion = reader.IsDBNull(3) ? null : reader.GetString(3),
          Telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
          Email = reader.IsDBNull(5) ? null : reader.GetString(5)
        };
      }

      return null;
    }

    public async Task<bool> UpdateAsync(Cliente cliente)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "UPDATE Cliente SET nombre = @nombre, apellido = @apellido, direccion = @direccion, telefono = @telefono, email = @email WHERE id = @id";

      cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
      cmd.Parameters.AddWithValue("@apellido", cliente.Apellido);
      cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);
      cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
      cmd.Parameters.AddWithValue("@email", cliente.Email);
      cmd.Parameters.AddWithValue("@id", cliente.Id);

      var rowsAffected = await cmd.ExecuteNonQueryAsync();
      return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      using var conn = _factory.CreateConnection();
      await conn.OpenAsync();

      var cmd = conn.CreateCommand();
      cmd.CommandText = "DELETE FROM Cliente WHERE id = @id";
      cmd.Parameters.AddWithValue("@id", id);

      var rowsAffected = await cmd.ExecuteNonQueryAsync();
      return rowsAffected > 0;
    }

  }
}
