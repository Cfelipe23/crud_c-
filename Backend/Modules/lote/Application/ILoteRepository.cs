using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClienteCRUD.Modules.Lotes.Domain;

namespace ClienteCRUD.Modules.Lotes.Application
{
  public interface ILoteRepository : IGenericRepository<Lote>
  {
  }
}
