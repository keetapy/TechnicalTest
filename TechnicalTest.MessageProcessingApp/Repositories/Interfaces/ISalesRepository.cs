using TechnicalTest.MessageProcessingApp.Models;

namespace TechnicalTest.MessageProcessingApp.Repositories.Interfaces;

public interface ISalesRepository
{
    bool AddSales(SalesEntity sale);
    void AdjustSales(ProductType? product, AdjustmentOperation? operation, decimal adjustmentAmount);
    IEnumerable<SalesEntity> GetAllRecords();
}
