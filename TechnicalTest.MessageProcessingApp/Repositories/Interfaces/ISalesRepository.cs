using TechnicalTest.MessageProcessingApp.Models;

namespace TechnicalTest.MessageProcessingApp.Repositories.Interfaces;

public interface ISalesRepository
{
    bool AddSales(SalesEntity sale);
    IEnumerable<SalesEntity> GetAllRecords();
}
