using Microsoft.Data.Sqlite;

namespace ClienteCRUD.Infrastructure.Persistence
{
  public class SqliteConnectionFactory
  {
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
      _connectionString = connectionString;
    }

    public SqliteConnection CreateConnection()
    {
      return new SqliteConnection(_connectionString);
    }
  }
}