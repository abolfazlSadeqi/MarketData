using Infra.EF;
using MarketFeedProcessorUI.Classes;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting up...");

    var builder = WebApplication.CreateBuilder(args);

 
    builder.Host.UseSerilog();  

    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var configuration = builder.Configuration.GetConnectionString("Redis");
        return ConnectionMultiplexer.Connect(configuration);
    });

   
    var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(defaultConnection));

    // HttpClient
    builder.Services.AddHttpClient("MarketApi", c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("MarketApi").Value);
    });

    // Add services & background jobs
    builder.RegisterServices();

    // Swagger & Controllers
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
