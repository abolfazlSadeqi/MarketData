using Core.Entites;
using Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infra.EF.Repositories;

public class SymbolAverageRepository : ISymbolAverageRepository
{
    private readonly AppDbContext _context;

    public SymbolAverageRepository(AppDbContext context) => _context = context;

    public async Task BulkAsync(List<SymbolAverage> SymbolAverages)
    {
        _context.SymbolAverage.AddRange(SymbolAverages);
       await _context.SaveChangesAsync();
    }

    public async Task<List<SymbolAverage>> GetAll()
      => await _context.SymbolAverage.ToListAsync();

  

}