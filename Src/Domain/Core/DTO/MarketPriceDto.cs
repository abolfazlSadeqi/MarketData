using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO;

public class MarketPriceDto
{
    public string Symbol { get; set; } = null!;
    public decimal Price { get; set; }
    public string Time { get; set; } = null!;
}