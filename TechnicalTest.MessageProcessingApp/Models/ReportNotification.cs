namespace TechnicalTest.MessageProcessingApp.Models;

public class ReportNotification
{
    public ProductType Product { get; set; }

    public int SalesNumber { get; set; }

    public decimal TotalValue { get; set; }

    public override string ToString()
    {
        return $"Product type: {Enum.GetName(Product)} \t " +
            $"Number of sales: {SalesNumber}\t " +
            $"Total value: {TotalValue}";
    }
}
