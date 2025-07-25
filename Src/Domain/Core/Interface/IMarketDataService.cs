using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface;

public interface IMarketDataService
{
    Task HandlePriceUpdateAsync(string symbolName, decimal price, int RecordCount);
}
