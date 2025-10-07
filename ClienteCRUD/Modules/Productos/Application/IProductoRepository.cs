using System.Collections.Generic;
using System.Threading.Tasks;
using ClienteCRUD.Shared.Interfaces;
using ClienteCRUD.Modules.Productos.Domain;

namespace ClienteCRUD.Modules.Productos.Application
{
  public interface IProductoRepository : IGenericRepository<Producto>
  {
  }
}