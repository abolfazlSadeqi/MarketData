using Core.Entites;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MarketDataFeedSimulator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMarketDataFeedService _feed;

    public MarketController(IMarketDataFeedService feed)
    {
        _feed = feed;
    }

    [HttpGet("GenerateSymbol")]
    public ActionResult<List<object>> GenerateSymbol()
    {
        return Ok(_feed.GenerateSymbol());
    }


    [HttpGet("prices")]
    public ActionResult<List<object>> GetRandomPrices()
    {
        var prices = _feed.GenerateRandomPrices();

        var response = prices.Select(p => new
        {
            Symbol = p.Symbol.Name,
            Price = p.Price,
            Time = p.RecordedAt.ToString("HH:mm:ss.fff")
        }).ToList();

        return Ok(response);
    }
}

 
