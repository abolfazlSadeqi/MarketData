using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain;

using Xunit;
using Core.Entites;
using System;

public class SymbolTests
{
    [Fact]
    public void Symbol_Should_Have_Correct_Name_And_Timestamps()
    {

        //Arrange
        var symbol = new Symbol("test");

        //act

        //Assert
        Assert.Equal("test", symbol.Name);
        Assert.True((DateTime.Now - symbol.CreatedAt).TotalSeconds < 1);
        Assert.Null(symbol.UpdatedAt);
    }

    [Fact]
    public void Symbol_Update_Should_Set_UpdatedAt()
    {

        //Arrange
        var symbol = new Symbol("test");

        //act

       
        symbol.Update();

        //Assert
        Assert.NotNull(symbol.UpdatedAt);
        Assert.True((DateTime.Now - symbol.UpdatedAt.Value).TotalSeconds < 1);
    }

  
}

