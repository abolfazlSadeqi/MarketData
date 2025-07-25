using Application.Services;
using Core.DTO;
using Core.Entites;
using Core.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Application;



public class MovingAverageBatchServiceTests
{
    [Fact]
    public async Task ProcessAllSymbolsMovingAverageAsync_ShouldCalculateAverages()
    {
        // Arrange
        var cacheMock = new Mock<ICacheService>();
        var repoMock = new Mock<ISymbolRepository>();
        var dbMock = new Mock<IAppDbContext>();
        var loggerMock = new Mock<ILogger<MovingAverageBatchService>>();
        var configMock = new Mock<IConfiguration>();
        var avgRepoMock = new Mock<ISymbolAverageRepository>();
        var sectionMock = new Mock<IConfigurationSection>();

        var symbol = new Symbol("test");
        var history = new List<SymbolPriceHistoryDTO>
        {
            new SymbolPriceHistoryDTO(){SymbolId= symbol.Id, Price= 100},
            new SymbolPriceHistoryDTO(){SymbolId= symbol.Id, Price= 200},
        };

        sectionMock.Setup(s => s.Value).Returns("2");
        configMock.Setup(c => c.GetSection("MovingAverageWindowSize")).Returns(sectionMock.Object);


        cacheMock.Setup(c => c.GetAllSymbolNamesFromHistoryAsync()).Returns(new List<string> { symbol.Name });
        cacheMock.Setup(c => c.GetListAsync<SymbolPriceHistoryDTO>($"symbol:{symbol.Name}:history", 2)).ReturnsAsync(history);


        //act
        var service = new MovingAverageBatchService(cacheMock.Object, repoMock.Object, dbMock.Object, configMock.Object, loggerMock.Object, avgRepoMock.Object);

        await service.ProcessAllSymbolsMovingAverageAsync();

        //Assert
        cacheMock.Verify(c =>
    c.SetAsync($"symbol:{symbol.Name}:average", 150m, It.IsAny<TimeSpan?>()),
    Times.Once);

        avgRepoMock.Verify(r => r.BulkAsync(It.IsAny<List<SymbolAverage>>()), Times.Once);
    }
}

