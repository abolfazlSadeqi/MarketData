using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Core.Interface;

public interface IMarketDataFeedService
{
    List<SymbolPrice> GenerateRandomPrices();
    List<Symbol> GenerateSymbol();

}
