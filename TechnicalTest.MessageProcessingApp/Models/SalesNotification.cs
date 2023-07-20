using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.MessageProcessingApp.Models;

public class SalesNotification
{
    public ProductType? Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
