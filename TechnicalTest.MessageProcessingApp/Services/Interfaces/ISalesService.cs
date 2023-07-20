using TechnicalTest.MessageProcessingApp.Models;

namespace TechnicalTest.MessageProcessingApp.Services.Interfaces;

public interface ISalesService
{
    bool AddSales(SalesNotification sale);
    IEnumerable<ReportNotification> GetAnalytics();
}
