using Core.DTO;
using Core.Entites;
using Core.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Application.Services;


/// <summary>
/// Calculate moving averages in real-time  
/// </summary>
public class MovingAverageBatchService : IMovingAverageService
{
    private readonly ICacheService _cache;
    private readonly ISymbolRepository _symbolRepo;
    private readonly ISymbolAverageRepository _symbolAverageRepository;

    private readonly IAppDbContext _db;
    private readonly ILogger<MovingAverageBatchService> _logger;
    private readonly int _windowSize;

    public MovingAverageBatchService(
        ICacheService cache,
        ISymbolRepository symbolRepo,
        IAppDbContext db,
        IConfiguration configuration,
        ILogger<MovingAverageBatchService> logger,
        ISymbolAverageRepository symbolAverageRepository
        )
    {
        _cache = cache;
        _symbolRepo = symbolRepo;
        _db = db;
        _logger = logger;
        _symbolAverageRepository = symbolAverageRepository;

        _windowSize = int.TryParse(configuration.GetSection("MovingAverageWindowSize")?.Value, out var value) ? value : 20;


    }

    /// <summary>
    /// Calculate moving averages in real-time 
    /// </summary>
    /// <returns></returns>
    public async Task ProcessAllSymbolsMovingAverageAsync()
    {
        _logger.LogInformation("Starting moving average calculation for all symbols...");

        var allSymbols = await GetAllSymbolsCachedAsync();
        var resultCollection = new ConcurrentBag<SymbolAverage>();

        await Parallel.ForEachAsync(allSymbols, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount 
        }, async (symbol, cancellationToken) =>
        {
            try
            {
                var historyKey = $"symbol:{symbol.Name}:history";
                var averageKey = $"symbol:{symbol.Name}:average";

                var history = await _cache.GetListAsync<SymbolPriceHistoryDTO>(historyKey, _windowSize);

                if (history.Count == 0)
                    return;

                var avg = history.Average(x => x.Price);

                await _cache.SetAsync(averageKey, avg);

                resultCollection.Add(new SymbolAverage(symbol.Id, avg, DateTime.Now));

                _logger.LogDebug("Symbol: {Symbol} -> Avg: {Avg}", symbol.Name, avg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average for symbol {Symbol}", symbol.Name);
            }
        });

        if (resultCollection.Any())
        {
            await _symbolAverageRepository.BulkAsync(resultCollection.ToList());
        }

        _logger.LogInformation("Completed moving average calculation for {Count} symbols", resultCollection.Count);
    }

    private async Task<List<Symbol>> GetAllSymbolsCachedAsync()
    {
        List<Symbol> symbols = new List<Symbol>();

        var _symbols = _cache.GetAllSymbolNamesFromHistoryAsync();

        if (_symbols is not null && _symbols.Any())
        {
            _logger.LogInformation("Fetched symbols from cache.");
            return _symbols.Select(d => new Symbol(d)).ToList();
        }


        symbols = await _symbolRepo.GetAll();


        return symbols;
    }

}

