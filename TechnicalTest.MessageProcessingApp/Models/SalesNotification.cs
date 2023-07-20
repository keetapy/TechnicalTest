namespace TechnicalTest.MessageProcessingApp.Models;

public class SalesNotification
{
    public ProductType? Product { get; set; }
    public AdjustmentOperation? AdjustmentOperation { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
