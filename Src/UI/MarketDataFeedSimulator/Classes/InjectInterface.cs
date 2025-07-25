using Core.Interface;
using Infrastructure.Services;

namespace MarketDataFeedSimulator.Classes;

public static class InjectInterface
{
    public static void Register(this WebApplicationBuilder builder)
    {

        builder.Services.AddSingleton<IMarketDataFeedService, MarketDataFeedService>();
      
    }
}
