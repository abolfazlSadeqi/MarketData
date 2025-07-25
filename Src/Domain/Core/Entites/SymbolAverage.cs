using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;

public class SymbolAverage
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid SymbolId { get; private set; }

    public decimal AveragePrice { get; private set; }

    public DateTime CalculatedAt { get; private set; }

    public SymbolAverage(Guid symbolId, decimal averagePrice, DateTime? time = null)
    {
        if (averagePrice <= 0)
            throw new DomainException("Average must be positive.");

        SymbolId = symbolId;
        AveragePrice = averagePrice;
        CalculatedAt = time ?? DateTime.Now;
    }

    protected SymbolAverage()
    { }
}

