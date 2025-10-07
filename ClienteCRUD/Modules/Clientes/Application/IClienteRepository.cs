using System.Collections.Generic;
using System.Threading.Tasks;
using ClienteCRUD.Shared.Interfaces;
using ClienteCRUD.Modules.Clientes.Domain;

namespace ClienteCRUD.Modules.Clientes.Application
{
  public interface IClienteRepository : IGenericRepository<Cliente>
  {
  }
}