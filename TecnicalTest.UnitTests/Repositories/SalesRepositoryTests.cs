using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Repositories;

namespace TecnicalTest.UnitTests.Repositories;

public class SalesRepositoryTests
{
    [Fact]
    public void AddSales_WhenProductExists_ShouldUpdateExistingSalesEntity()
    {
        // Arrange
        var repository = new SalesRepository();
        var initialSalesEntity = new SalesEntity { Product = ProductType.Apple, Price = 10, SalesNumber = 5 };
        repository.AddSales(initialSalesEntity);
        var updatedSalesEntity = new SalesEntity { Product = ProductType.Apple, Price = 15, SalesNumber = 3 };

        // Act
        var result = repository.AddSales(updatedSalesEntity);

        // Assert
        Assert.True(result);
        var updatedEntity = repository.GetAllRecords().Single();
        Assert.Equal(updatedSalesEntity.Price, updatedEntity.Price);
        Assert.Equal(8, updatedEntity.SalesNumber);
    }

    [Fact]
    public void AddSales_WhenProductDoesNotExist_ShouldAddNewSalesEntity()
    {
        // Arrange
        var repository = new SalesRepository();
        var salesEntity = new SalesEntity { Product = ProductType.Apple, Price = 10, SalesNumber = 5 };

        // Act
        var result = repository.AddSales(salesEntity);

        // Assert
        Assert.True(result);
        var addedEntity = repository.GetAllRecords().Single();
        Assert.Equal(salesEntity.Product, addedEntity.Product);
        Assert.Equal(salesEntity.Price, addedEntity.Price);
        Assert.Equal(salesEntity.SalesNumber, addedEntity.SalesNumber);
    }
}
