using System.Collections.Generic;
using System.Threading.Tasks;
using crud_c_.Shared.Interfaces;
using crud_c_.Modules.Productos.Domain;

namespace crud_c_.Modules.Productos.Application
{
  public interface IProductoRepository : IGenericRepository<Producto>
  {
  }
}