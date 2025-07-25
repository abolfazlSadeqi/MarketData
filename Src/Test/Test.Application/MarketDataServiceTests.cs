using Application;
using Core.Entites;
using Core.Interface;
using global::Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test.Application;



public class MarketDataServiceTests
{
    [Fact]
    public async Task HandlePriceUpdateAsync_ShouldUpdatePriceAndHistory()
    {
        // Arrange

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var scopeMock = new Mock<IServiceScope>();
        var providerMock = new Mock<IServiceProvider>();
        var cacheMock = new Mock<ICacheService>();
        var loggerMock = new Mock<ILogger<MarketDataService>>();
        var dbContextMock = new Mock<IAppDbContext>();
        var symbolRepoMock = new Mock<ISymbolRepository>();
        var priceRepoMock = new Mock<ISymbolPriceRepository>();

        var symbolName = "test";
        var price = 1234.56m;
        var symbol = new Symbol(symbolName);

        symbolRepoMock.Setup(s => s.AddOrUpdateAsync(It.IsAny<Symbol>())).ReturnsAsync(symbol);
        providerMock.Setup(p => p.GetService(typeof(IAppDbContext))).Returns(dbContextMock.Object);
        providerMock.Setup(p => p.GetService(typeof(ISymbolRepository))).Returns(symbolRepoMock.Object);
        providerMock.Setup(p => p.GetService(typeof(ISymbolPriceRepository))).Returns(priceRepoMock.Object);
        scopeMock.Setup(s => s.ServiceProvider).Returns(providerMock.Object);
        scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

        var service = new MarketDataService(scopeFactoryMock.Object, cacheMock.Object, loggerMock.Object);


        //Act

        await service.HandlePriceUpdateAsync(symbolName, price,10);


        //Assert
        symbolRepoMock.Verify(r => r.AddOrUpdateAsync(It.IsAny<Symbol>()), Times.Once);
        cacheMock.Verify(c => c.SetAsync($"symbol:{symbolName}:latest", It.IsAny<SymbolPrice>(), It.IsAny<TimeSpan>()), Times.Once);
        cacheMock.Verify(c => c.AppendToListAsync($"symbol:{symbolName}:history", It.IsAny<SymbolPriceHistory>(), 1000), Times.Once);
    }
}

