using crud_c_.Modules.Clientes.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace crud_c_.Swagger.Examples
{
  public class CreateClienteRequestExample : IExamplesProvider<Cliente>
  {
    public Cliente GetExamples()
    {
      return new Cliente
      {
        Nombre = "Juan",
        Apellido = "Pérez",
        Direccion = "Calle 123",
        Telefono = "555-1234",
        Email = "juan@example.com"
      };
    }
  }

  public class ClienteResponseExample : IExamplesProvider<Cliente>
  {
    public Cliente GetExamples()
    {
      return new Cliente
      {
        Id = 1,
        Nombre = "Juan",
        Apellido = "Pérez",
        Direccion = "Calle 123",
        Telefono = "555-1234",
        Email = "juan@example.com"
      };
    }
  }
}
