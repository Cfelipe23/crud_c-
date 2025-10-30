using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using crud_c_.Shared.Interfaces;
using crud_c_.Modules.Lotes.Domain;

namespace crud_c_.Modules.Lotes.Application
{
  public interface ILoteRepository : IGenericRepository<Lote>
  {
  }
}