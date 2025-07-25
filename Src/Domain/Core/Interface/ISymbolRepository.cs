using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface;

public interface ISymbolRepository
{
    Task<Symbol?> GetByNameAsync(string name);
    Task<Symbol> AddOrUpdateAsync(Symbol symbol);

    Task<List<Symbol>> GetAll();
}

