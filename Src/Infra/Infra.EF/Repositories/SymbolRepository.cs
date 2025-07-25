using Core.Entites;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.EF.Repositories;

public class SymbolRepository : ISymbolRepository
{
    private readonly AppDbContext _context;

    public SymbolRepository(AppDbContext context) => _context = context;

    public async Task<Symbol?> GetByNameAsync(string name)
        => await _context.Symbols.FirstOrDefaultAsync(s => s.Name == name);

    public async Task<List<Symbol>> GetAll()
      => await _context.Symbols.ToListAsync();
    public async Task<Symbol> AddOrUpdateAsync(Symbol symbol)
    {
        var existing = await GetByNameAsync(symbol.Name);
        if (existing != null)
        {
            existing.Update();
            _context.Symbols.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        _context.Symbols.Add(symbol);
        await _context.SaveChangesAsync();
        return symbol;
    }
}
