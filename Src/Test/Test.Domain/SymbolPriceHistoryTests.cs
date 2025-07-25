using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain;

using Xunit;
using Core.Entites;
using System;

public class SymbolPriceHistoryTests
{
    [Fact]
    public void SymbolPriceHistory_Should_Set_Correct_Defaults()
    {
        //Arrange
        var symbolId = Guid.NewGuid();

        //act
        var history = new SymbolPriceHistory(symbolId, 1000.00m);

        //Assert
        Assert.Equal(symbolId, history.SymbolId);
        Assert.Equal(1000.00m, history.Price);
        Assert.True((DateTime.Now - history.RecordedAt).TotalSeconds < 1);
    }

    [Fact]
    public void SymbolPriceHistory_Throws_When_Price_Invalid()
    {
        //Arrange
        var symbolId = Guid.NewGuid();

        //Act

        //Assert
        Assert.Throws<ArgumentException>(() => new SymbolPriceHistory(symbolId, 0));
    }

    [Fact]
    public void SymbolPriceHistory_Should_Use_Provided_Timestamp()
    {
        //Arrange
        var symbolId = Guid.NewGuid();
        var timestamp = new DateTime(2024, 1, 1);


        //Act
        var history = new SymbolPriceHistory(symbolId, 123.45m, timestamp);


        

        //Assert
        Assert.Equal(timestamp, history.RecordedAt);
    }
}

