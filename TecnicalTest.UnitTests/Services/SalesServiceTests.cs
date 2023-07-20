using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Repositories.Interfaces;
using TechnicalTest.MessageProcessingApp.Services;

namespace TechnicalTest.UnitTests.Services;

public class SalesServiceTests
{
    [Fact]
    public void AddSales_WhenProductIsNotNull_ShouldAddSalesEntity()
    {
        // Arrange
        var mockRepository = new Mock<ISalesRepository>();
        var service = new SalesService(mockRepository.Object);
        var sale = new SalesNotification { Product = ProductType.Apple, Price = 10, Quantity = 5 };

        // Act
        var result = service.AddSales(sale);

        // Assert
        mockRepository.Verify(r => r.AddSales(It.IsAny<SalesEntity>()), Times.Once);
    }

    [Fact]
    public void AddSales_WhenProductIsNull_ShouldNotAddSalesEntity()
    {
        // Arrange
        var mockRepository = new Mock<ISalesRepository>();
        var service = new SalesService(mockRepository.Object);
        var sale = new SalesNotification { Product = null, Price = 10, Quantity = 5 };

        // Act
        var result = service.AddSales(sale);

        // Assert
        Assert.False(result);
        mockRepository.Verify(r => r.AddSales(It.IsAny<SalesEntity>()), Times.Never);
    }

    [Fact]
    public void GetAnalytics_ShouldReturnReportNotifications()
    {
        // Arrange
        var mockRepository = new Mock<ISalesRepository>();
        var service = new SalesService(mockRepository.Object);
        var sales = new List<SalesEntity>
            {
                new SalesEntity { Product = ProductType.Apple, Price = 10, SalesNumber = 5 },
                new SalesEntity { Product = ProductType.Banana, Price = 8, SalesNumber = 3 }
            };
        mockRepository.Setup(r => r.GetAllRecords()).Returns(sales);

        // Act
        var analytics = service.GetAnalytics();

        // Assert
        Assert.Equal(2, analytics.Count());
        Assert.Contains(analytics, x => x.Product == ProductType.Apple && x.SalesNumber == 5 && x.TotalValue == 50);
        Assert.Contains(analytics, x => x.Product == ProductType.Banana && x.SalesNumber == 3 && x.TotalValue == 24);
    }

    [Fact]
    public void AdjustSales_ShouldApplyAdjustmentToAllSalesOfProductType()
    {
        // Arrange
        var mockRepository = new Mock<ISalesRepository>();
        var service = new SalesService(mockRepository.Object);
        var product = ProductType.Apple;
        var operation = AdjustmentOperation.Add;
        var adjustmentAmount = 2;

        // Act
        service.AdjustSales(product, operation, adjustmentAmount);

        // Assert
        mockRepository.Verify(r => r.AdjustSales(product, operation, adjustmentAmount), Times.Once);
    }
}
