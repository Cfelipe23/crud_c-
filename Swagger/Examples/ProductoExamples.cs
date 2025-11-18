using crud_c_.Modules.Productos.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace crud_c_.Swagger.Examples
{
  public class CreateProductoRequestExample : IExamplesProvider<Producto>
  {
    public Producto GetExamples()
    {
      return new Producto
      {
        Nombre = "Prod Prueba",
        Descripcion = "Demo",
        Precio = 10.50m,
        Stock = 100
      };
    }
  }

  public class ProductoResponseExample : IExamplesProvider<Producto>
  {
    public Producto GetExamples()
    {
      return new Producto
      {
        Id = 1,
        Nombre = "Prod Prueba",
        Descripcion = "Demo",
        Precio = 10.50m,
        Stock = 100
      };
    }
  }
}
