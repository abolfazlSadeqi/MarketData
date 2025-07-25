using Application;
using Application.Services;
using Core.Interface;
using Infra.BackgroundJobs;
using Infra.Cache;
using Infra.EF;
using Infra.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog;
using Serilog.Events;

namespace MarketFeedProcessorUI.Classes;

public static class Register
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddScoped<IMarketDataService, MarketDataService>();
        builder.Services.AddScoped<ICacheService, RedisCacheService>();
        builder.Services.AddScoped<ISymbolRepository, SymbolRepository>();
        builder.Services.AddScoped<ISymbolPriceRepository, SymbolPriceRepository>();

        builder.Services.AddScoped<IMovingAverageService, MovingAverageBatchService>();

        builder.Services.AddScoped<ISymbolAverageRepository, SymbolAverageRepository>();
        builder.Services.AddScoped<IAnomalyDetectionService, AnomalyDetectionService>();

        builder.Services.AddHostedService<MarketFeedProcessor>();

        builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

    }

   

}
