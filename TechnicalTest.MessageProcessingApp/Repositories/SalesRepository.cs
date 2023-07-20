using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Repositories.Interfaces;

namespace TechnicalTest.MessageProcessingApp.Repositories;

public class SalesRepository: ISalesRepository
{
    private List<SalesEntity> _sales { get; set; }

    public SalesRepository()
    {
        _sales = new List<SalesEntity>();
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

        Console.WriteLine("Added new product type: [{0}]",Enum.GetName(sale.Product));
        _sales.Add(sale);

        return true;
    }

    public IEnumerable<SalesEntity> GetAllRecords()
    {
        return _sales.OrderBy(x => x.Product).ToList();  
    }
}
