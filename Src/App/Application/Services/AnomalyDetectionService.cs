using Core.DTO;
using Core.Entites;
using Core.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AnomalyDetectionService : IAnomalyDetectionService
{
    private readonly ICacheService _cache;
    private readonly ILogger<AnomalyDetectionService> _logger;
    private readonly double _spikeThreshold = 0.02;

    public AnomalyDetectionService(ICacheService cache, ILogger<AnomalyDetectionService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<AnomalyResult>> DetectAsync()
    {
        var anomalies = new ConcurrentBag<AnomalyResult>();
        var now = DateTime.Now;

        var symbolNames =  _cache.GetAllSymbolNamesFromHistoryAsync();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount * 2
        };

        await Parallel.ForEachAsync(symbolNames, options, async (symbol, _) =>
        {
            try
            {
                var historyKey = $"symbol:{symbol}:history";

                var cachedHistory = await _cache.GetListAsync<SymbolPriceHistoryDTO>(historyKey);

                if (cachedHistory is null || cachedHistory.Count < 2)
                    return;

                var recent = cachedHistory
                   .Where(p => (now - p.RecordedAt).TotalSeconds <= 1)
                    .OrderBy(p => p.RecordedAt)
                    .ToList();

                if (recent.Count < 2)
                    return;

                var first = recent.First().Price;
                var last = recent.Last().Price;

                if (first == 0) return;

                var change = (last - first) / first;

                if (Math.Abs(change) > (decimal)_spikeThreshold)
                {
                    var result = new AnomalyResult(symbol, first, last, change * 100);
                    anomalies.Add(result);

                    await _cache.SetAsync($"symbol:{symbol}:anomaly", result, TimeSpan.FromMinutes(2))
                                .ConfigureAwait(false);

                    _logger.LogWarning("Anomaly detected for {Symbol}: {Change:P2}", symbol, change);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing symbol {Symbol}", symbol);
            }
        });

        return anomalies.ToList();
    }

}

