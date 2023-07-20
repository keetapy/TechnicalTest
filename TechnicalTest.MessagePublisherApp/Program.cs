using Microsoft.Extensions.Configuration;
using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessagePublisherApp;

IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .Build();

var hostname = configuration.GetSection("RabbitMq")["HostName"];
var queueName = configuration.GetSection("RabbitMq")["QueueName"];

var publisher = new SalesNotificationPublisher(hostname, queueName);

var usersResult = "add";

while (usersResult?.ToLower() != "stop")
{
    Console.WriteLine("Product type (0-Apple; 1-Banana): ");
    var product = Console.ReadLine();

    var productType = product switch
    {
        "0" => ProductType.Apple,
        "1" => ProductType.Banana,
        _ => ProductType.Apple
    };

    Console.WriteLine("Quantity: ");
    var quantity = int.Parse(Console.ReadLine());

    Console.WriteLine("Price: ");
    var price = decimal.Parse(Console.ReadLine());

    publisher.SendNotification(new SalesNotification
    {
        Product = productType,
        Quantity = quantity,
        Price = price
    });

    Console.WriteLine("Whould you like to add produt?");
    Console.WriteLine("Enter \"stop\" to stop app");
    usersResult = Console.ReadLine();
}
