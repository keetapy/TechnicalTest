using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.MessageProcessingApp.Models;

public class SalesEntity
{
    public ProductType Product { get; set; }

    public int SalesNumber { get; set; }

    public decimal Price { get; set; }
}
