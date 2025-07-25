using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application;
using Core.DTO;
using Core.Interface;
using global::Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AnomalyDetectionServiceTests
{
    private readonly Mock<ICacheService> _cacheMock = new();
    private readonly Mock<ILogger<AnomalyDetectionService>> _loggerMock = new();

    [Fact]
    public async Task DetectAsync_ShouldReturnAnomalies_WhenSpikeDetected()
    {
        // Arrange
        var service = new AnomalyDetectionService(_cacheMock.Object, _loggerMock.Object);
        var symbol = "test";

        var configMock = new Mock<IConfiguration>();
        var sectionMock = new Mock<IConfigurationSection>();

        sectionMock.Setup(s => s.Value).Returns("2");
        configMock.Setup(c => c.GetSection("MovingAverageWindowSize")).Returns(sectionMock.Object);

        _cacheMock.Setup(c => c.GetAllSymbolNamesFromHistoryAsync())
                  .Returns(new List<string> { symbol });


        var history = new List<SymbolPriceHistoryDTO>
        {
            new SymbolPriceHistoryDTO(){
                SymbolId= Guid.NewGuid(),

                Price= 100,
                RecordedAt= DateTime.Now.AddMilliseconds(-900)

            }
        };

        _cacheMock.Setup(c => c.GetListAsync<SymbolPriceHistoryDTO>($"symbol:{symbol}:history",1000))
                  .ReturnsAsync(history);

        // Act
        var result = await service.DetectAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(symbol, result[0].Symbol);
    }
}

