using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;


public class SymbolPrice
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid SymbolId { get; private set; }
    public Symbol Symbol { get; private set; }

    public decimal Price { get; private set; }

    public DateTime RecordedAt { get; private set; }

    public void ChangePrice(decimal price)
    {
        Price = price;
        RecordedAt = DateTime.Now;
    }
    public SymbolPrice(Symbol symbol, decimal price)
    {
        if (price <= 0)
            throw new DomainException("Price must be positive.");

        SymbolId = symbol.Id;
        Price = price;
        RecordedAt = DateTime.Now;
        Symbol = symbol;
    }
    protected SymbolPrice()
    { }
}

