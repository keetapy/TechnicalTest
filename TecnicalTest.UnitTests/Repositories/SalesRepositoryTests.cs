using Microsoft.Extensions.Logging;
using Moq;
using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Repositories;

namespace TecnicalTest.UnitTests.Repositories;

public class SalesRepositoryTests
{
    [Fact]
    public void AddSales_WhenProductExists_ShouldUpdateExistingSalesEntity()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<SalesRepository>>();
        var repository = new SalesRepository(mockLogger.Object);
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
        var mockLogger = new Mock<ILogger<SalesRepository>>();
        var repository = new SalesRepository(mockLogger.Object);
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

    [Fact]
    public void AdjustSales_ShouldApplyAdjustmentToSalesOfSpecifiedProductType()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<SalesRepository>>();
        var repository = new SalesRepository(mockLogger.Object);
        var product = ProductType.Apple;
        var initialSale = new SalesEntity { Product = product, Price = 10, SalesNumber = 5 };
        repository.AddSales(initialSale);
        var adjustmentOperation = AdjustmentOperation.Add;
        var adjustmentAmount = 2.5m;

        // Act
        repository.AdjustSales(product, adjustmentOperation, adjustmentAmount);

        // Assert
        var sales = repository.GetAllRecords();
        var adjustedSale = sales.Single();
        Assert.Equal(12.5m, adjustedSale.Price);
    }
}
