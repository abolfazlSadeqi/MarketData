using Core.Entites;
using Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infra.EF.Repositories;

public class SymbolPriceRepository : ISymbolPriceRepository
{
    private readonly AppDbContext _db;
    public SymbolPriceRepository(AppDbContext db) => _db = db;

    public Task<SymbolPrice?> GetBySymbolIdAsync(Guid symbolId) =>
        _db.SymbolPrice.FirstOrDefaultAsync(p => p.SymbolId == symbolId);

    public Task<SymbolPrice?> GetBySymbolNameAsync(string SymbolName) =>
       _db.SymbolPrice.FirstOrDefaultAsync(p => p.Symbol.Name == SymbolName);

    public async Task AddOrUpdateAsync(SymbolPrice price)
    {
        var existing = await GetBySymbolNameAsync(price?.Symbol?.Name);
        if (existing is null)
            await _db.SymbolPrice.AddAsync(price);
        else
        {
            existing.ChangePrice(price.Price);
        }
    }

    public async Task AddHistoryAsync(SymbolPriceHistory history) => _db.SymbolPriceHistory.AddAsync(history).AsTask();
    public async Task BulkAddHistoryAsync(List< SymbolPriceHistory> historys) => _db.SymbolPriceHistory.AddRangeAsync(historys);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}

