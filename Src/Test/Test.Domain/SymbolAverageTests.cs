using System;
using System.Collections.Generic;
using Xunit;
using Core.Entites;
using System;

namespace Test.Domain;



public class SymbolAverageTests
{
    [Fact]
    public void SymbolAverage_Should_Set_Fields_Correctly()
    {
        //Arrange
        var symbolId = Guid.NewGuid();

        //act
        var average = new SymbolAverage(symbolId, 123.45m);

        //Assert
        Assert.Equal(symbolId, average.SymbolId);
        Assert.Equal(123.45m, average.AveragePrice);
        Assert.True((DateTime.Now - average.CalculatedAt).TotalSeconds < 1);
    }

    [Fact]
    public void SymbolAverage_Negative_Throws()
    {
        //Arrange
        var symbolId = Guid.NewGuid();

        //act

        //Assert
        Assert.Throws<ArgumentException>(() => new SymbolAverage(symbolId, 0));
    }
}
