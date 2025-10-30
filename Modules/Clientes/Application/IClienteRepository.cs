using System.Collections.Generic;
using System.Threading.Tasks;
using crud_c_.Shared.Interfaces;
using crud_c_.Modules.Clientes.Domain;

namespace crud_c_.Modules.Clientes.Application
{
  public interface IClienteRepository : IGenericRepository<Cliente>
  {
  }
}