using System;
using crud_c_.Modules.Lotes.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace crud_c_.Swagger.Examples
{
  public class CreateLoteRequestExample : IExamplesProvider<Lote>
  {
    public Lote GetExamples()
    {
      return new Lote
      {
        Codigo = "L-001",
        FechaIngreso = DateTime.Today,
        Cantidad = 50,
        ProductoId = 1
      };
    }
  }

  public class LoteResponseExample : IExamplesProvider<Lote>
  {
    public Lote GetExamples()
    {
      return new Lote
      {
        Id = 1,
        Codigo = "L-001",
        FechaIngreso = DateTime.Today,
        Cantidad = 50,
        ProductoId = 1
      };
    }
  }
}
