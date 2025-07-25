using Core.Entites;

namespace Core.Interface;

public interface ISymbolPriceRepository
{
    Task<SymbolPrice?> GetBySymbolIdAsync(Guid symbolId);
    Task AddOrUpdateAsync(SymbolPrice price);
    Task AddHistoryAsync(SymbolPriceHistory history);
    Task SaveChangesAsync();
    Task BulkAddHistoryAsync(List<SymbolPriceHistory> historys);
}


