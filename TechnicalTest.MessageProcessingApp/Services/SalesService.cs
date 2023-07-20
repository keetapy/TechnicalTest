using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Repositories;

namespace TechnicalTest.MessageProcessingApp.Services;

public class SalesService
{
    private readonly SalesRepository _salesRepository;

    public SalesService()
    {
         _salesRepository = new SalesRepository();
    }

    public bool AddSales(SalesNotification sale)
    {
        if(sale.Product is null)
        {
            return false;
        }

        var itemToAdd = new SalesEntity()
        {
            Product = sale.Product ?? ProductType.Apple,
            Price = sale.Price,
            SalesNumber = sale.Quantity > 0 ? sale.Quantity : 1
        };

        return _salesRepository.AddSales(itemToAdd);
    }

    public IEnumerable<ReportNotification> GetAnalytics()
    {
        var data = _salesRepository.GetAllRecords();

        return data.Select(x => new ReportNotification()
        {
            Product = x.Product,
            SalesNumber = x.SalesNumber,
            TotalValue = x.SalesNumber * x.Price
        });
    }
}
