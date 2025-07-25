using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entites;


public class SymbolPriceHistory
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid SymbolId { get; private set; }

    public decimal Price { get; private set; }

    public DateTime RecordedAt { get; private set; }

  
    public SymbolPriceHistory(Guid symbolId, decimal price, DateTime? timestamp = null)
    {
        if (price <= 0)
            throw new DomainException("Price must be positive.");

        SymbolId = symbolId;
        Price = price;
        RecordedAt = timestamp ?? DateTime.Now;
    }


    public SymbolPriceHistory(Guid symbolId, decimal price, DateTime recordedAt)
    {
        if (price <= 0)
            throw new DomainException("Price must be positive.");

        SymbolId = symbolId;
        Price = price;
        RecordedAt = recordedAt;
    }
    protected SymbolPriceHistory()
    { }
}

