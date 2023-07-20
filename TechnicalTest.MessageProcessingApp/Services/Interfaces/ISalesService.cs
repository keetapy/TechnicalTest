using TechnicalTest.MessageProcessingApp.Models;

namespace TechnicalTest.MessageProcessingApp.Services.Interfaces;

public interface ISalesService
{
    bool AddSales(SalesNotification sale);
    void AdjustSales(ProductType? product, AdjustmentOperation? operation, decimal adjustmentAmount);
    IEnumerable<ReportNotification> GetAnalytics();
}
