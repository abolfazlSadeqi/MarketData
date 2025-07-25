
using Core.Entites;
using Core.Interface;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Infrastructure.Services;

public class MarketDataFeedService : IMarketDataFeedService
{
    private readonly string[] _symbols;
    private readonly ConcurrentDictionary<string, Symbol> _symbolMap = new();
    private readonly ConcurrentDictionary<string, decimal> _lastPrices = new();
    private readonly Random _rand = new();

    public MarketDataFeedService()
    {
        _symbols = Enumerable.Range(1, 10).Select(i => $"SYM{i:D4}").ToArray();

        foreach (var symbolName in _symbols)
        {
            var symbol = new Symbol(symbolName);
            _symbolMap[symbolName] = symbol;
            _lastPrices[symbolName] = 100m + (decimal)(_rand.NextDouble() * 50);
        }
    }
    public List<Symbol> GenerateSymbol()
    {
        return _symbolMap.Select(d =>  d.Value  ).ToList();
    }
        
    public List<SymbolPrice> GenerateRandomPrices()
    {
        var now = DateTime.Now;
        var result = new List<SymbolPrice>(_symbols.Length);

        foreach (var symbol in _symbols)
        {
            var lastPrice = _lastPrices[symbol];
            var delta = (decimal)(_rand.NextDouble() - 0.5) * 0.7m;
            var newPrice = Math.Round(lastPrice + delta, 2);
            newPrice = Math.Max(1, newPrice);

            _lastPrices[symbol] = newPrice;

            var symbolEntity = _symbolMap[symbol];
            var price = new SymbolPrice(symbolEntity, newPrice);

            result.Add(price);
        }

        return result;
    }
}

