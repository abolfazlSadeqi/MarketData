using Core.DTO;
using Core.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Infra.BackgroundJobs;

public class MarketFeedProcessor : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MarketFeedProcessor> _logger;

    public MarketFeedProcessor(
        IHttpClientFactory httpClientFactory,
        IServiceScopeFactory scopeFactory,
        ILogger<MarketFeedProcessor> logger)
    {
        _httpClientFactory = httpClientFactory;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MarketFeedProcessor started.");

        var client = _httpClientFactory.CreateClient("MarketApi");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await client.GetFromJsonAsync<List<MarketPriceDto>>("api/Market/prices", cancellationToken: stoppingToken);

                if (response != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var marketService = scope.ServiceProvider.GetRequiredService<IMarketDataService>();

                    var tasks = response.Select(data =>
                        marketService.HandlePriceUpdateAsync(data.Symbol, data.Price, response.Count)
                    );

                    await Task.WhenAll(tasks);
                }

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در پردازش قیمت‌ها");
            }
        }

        _logger.LogInformation("MarketFeedProcessor stopped.");
    }
}
