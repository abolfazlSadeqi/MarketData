
using Xunit;
using Core.Entites;
using System;


namespace Test.Domain;

public class SymbolPriceTests
{
    [Fact]
    public void SymbolPrice_Should_Set_Initial_Values()
    {
        //Arrange
        var symbol = new Symbol("test");
        var price = new SymbolPrice(symbol, 5000.00m);

        //act

        //Assert
        Assert.Equal(symbol.Id, price.SymbolId);
        Assert.Equal(5000.00m, price.Price);
        Assert.Equal(symbol, price.Symbol);
        Assert.True((DateTime.Now - price.RecordedAt).TotalSeconds < 1);
    }

    [Fact]
    public void SymbolPrice_ChangePrice_Should_Update_Price_And_Timestamp()
    {
        //Arrange
        var symbol = new Symbol("test");
        var price = new SymbolPrice(symbol, 100);
        //act

    
        price.ChangePrice(200);

        //Assert
        Assert.Equal(200, price.Price);
        Assert.True((DateTime.Now - price.RecordedAt).TotalSeconds < 1);
    }

    [Fact]
    public void SymbolPrice_Throws_When_Negative()
    {
        //Arrange
        var symbol = new Symbol("test");

        //Act

        //Assert
        Assert.Throws<ArgumentException>(() => new SymbolPrice(symbol, 0));
    }
}
