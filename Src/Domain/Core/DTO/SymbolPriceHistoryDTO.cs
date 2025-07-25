using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO;

public class SymbolPriceHistoryDTO
{
    public Guid Id { get;  set; }

    public Guid SymbolId { get;  set; }

    public decimal Price { get;  set; }

    public DateTime RecordedAt { get;  set; }
}
