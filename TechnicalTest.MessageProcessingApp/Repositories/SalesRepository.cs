using Microsoft.Extensions.Logging;
using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Repositories.Interfaces;

namespace TechnicalTest.MessageProcessingApp.Repositories;

public class SalesRepository: ISalesRepository
{
    private readonly ILogger<SalesRepository> _logger;
    private List<SalesEntity> _sales { get; set; }

    public SalesRepository(ILogger<SalesRepository> logger)
    {
        _sales = new List<SalesEntity>();
        _logger = logger;
    }

    public bool AddSales(SalesEntity sale)
    {
        var itemToUpdate = _sales.FirstOrDefault(x => x.Product.Equals(sale.Product));
        if (itemToUpdate is not null)
        {
            itemToUpdate.Price = sale.Price;
            itemToUpdate.SalesNumber += sale.SalesNumber;

            return true;
        }

        _logger.LogInformation("Added new product type: [{0}]", Enum.GetName(sale.Product));
        _sales.Add(sale);

        return true;
    }

    public void AdjustSales(
        ProductType? product, 
        AdjustmentOperation? operation, 
        decimal adjustmentAmount)
    {
        if (product is null || operation is null)
        {
            _logger.LogWarning("Product does not specified");
            return;
        }

        var affectedSale = _sales.FirstOrDefault(x => x.Product.Equals(product));

        if (affectedSale is null)
        {
            _logger.LogWarning("Product [{0}] does not exist",
                Enum.GetName(product ?? ProductType.None));
            return;
        }

        switch (operation)
        {
            case AdjustmentOperation.Add:
                affectedSale.Price += adjustmentAmount;
                break;
            case AdjustmentOperation.Subtract:
                affectedSale.Price -= adjustmentAmount;
                break;
            case AdjustmentOperation.Multiply:
                affectedSale.Price *= adjustmentAmount;
                break;
        }

        _logger.LogInformation("Adjusted sales for product type [{0}]: Operation={1}, AdjustmentAmount={2}", 
            Enum.GetName(product ?? ProductType.None), 
            operation, 
            adjustmentAmount);
    }

    public IEnumerable<SalesEntity> GetAllRecords()
    {
        return _sales.OrderBy(x => x.Product).ToList();  
    }
}
