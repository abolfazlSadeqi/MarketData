using Core.Entites;
using Core.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;

namespace Application;

/// <summary>
/// ذخیره قیمت نمادها در دیتابیس
/// </summary>
public class MarketDataService : IMarketDataService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ICacheService _cache;
    private readonly ILogger<MarketDataService> _logger;
    private static readonly ConcurrentDictionary<string, List<SymbolPriceHistory>> _historyBuffer = new();
    private static readonly ConcurrentDictionary<string, SymbolPrice> _priceBuffer = new();

    public MarketDataService(
        IServiceScopeFactory scopeFactory,
        ICacheService cache,
        ILogger<MarketDataService> logger)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
        _logger = logger;
    }
    public async Task HandlePriceUpdateAsync(string symbolName, decimal price, int RecordCount)
    {
        _logger.LogInformation("start HandlePriceUpdateAsync");

        var symbol = await _cache.GetAsync<Symbol>($"symbol:{symbolName}:entity");
        if (symbol == null)
        {
            using var scope = _scopeFactory.CreateScope();
            var symbolRepo = scope.ServiceProvider.GetRequiredService<ISymbolRepository>();
            symbol = await symbolRepo.AddOrUpdateAsync(new Symbol(symbolName));

            await _cache.SetAsync($"symbol:{symbolName}:entity", symbol, TimeSpan.FromHours(1));
        }

        var symbolId = symbol.Id;
        var newPrice = new SymbolPrice(symbol, price);
        var history = new SymbolPriceHistory(symbolId, price, DateTime.Now);

        await _cache.SetAsync($"symbol:{symbolName}:latest", newPrice, TimeSpan.FromMinutes(10));
        await _cache.AppendToListAsync($"symbol:{symbolName}:history", history, maxLength: 1000);


        _priceBuffer[symbolName] = newPrice;

        if (!_historyBuffer.ContainsKey(symbolName))
            _historyBuffer[symbolName] = new List<SymbolPriceHistory>();

        _historyBuffer[symbolName].Add(history);


        if (_historyBuffer[symbolName].Count >= RecordCount)
        {
            await FlushBufferToDatabaseAsync();
        }


        _logger.LogInformation("Symbol {Symbol} updated with price {Price}", symbolName, price);
    }

    private async Task FlushBufferToDatabaseAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var priceRepo = scope.ServiceProvider.GetRequiredService<ISymbolPriceRepository>();
        try
        {
            foreach (var symbol in _priceBuffer)
            {
                if (_priceBuffer.TryGetValue(symbol.Key, out var latestPrice) &&
                    _historyBuffer.TryGetValue(symbol.Key, out var historyList) &&
                    historyList.Any())
                {

                    await priceRepo.AddOrUpdateAsync(latestPrice);
                    await priceRepo.BulkAddHistoryAsync(historyList);
                }
            }

            await db.SaveChangesAsync();

            _historyBuffer.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error flushing buffer to DB for symbol ");
        }
    }




}
