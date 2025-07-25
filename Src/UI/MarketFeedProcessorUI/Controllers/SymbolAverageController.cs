using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketFeedProcessorUI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SymbolAverageController : ControllerBase
{
    private readonly IMovingAverageService _feed;
    private readonly IAnomalyDetectionService _anomalyService;

    public SymbolAverageController(IMovingAverageService feed, IAnomalyDetectionService anomalyService)
    {
        _feed = feed;
        _anomalyService = anomalyService;
    }
    [HttpGet("GenerateSymbol")]
    public async Task<ActionResult<List<object>>> GenerateSymbol()
    {
        await _feed.ProcessAllSymbolsMovingAverageAsync();
        return Ok();
    }



    [HttpGet("anomalies")]
    public async Task<IActionResult> GetAnomalies()
    {
        var result = await _anomalyService.DetectAsync();
        return Ok(result);
    }
}
