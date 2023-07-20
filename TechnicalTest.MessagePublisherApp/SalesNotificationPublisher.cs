using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TechnicalTest.MessageProcessingApp.Models;

namespace TechnicalTest.MessagePublisherApp;

public class SalesNotificationPublisher
{
    private readonly string _hostname;
    private readonly string _queueName;

    public SalesNotificationPublisher(string hostname, string queueName)
    {
        _hostname = hostname;
        _queueName = queueName;
    }

    public void SendNotification(SalesNotification notification)
    {
        var factory = new ConnectionFactory() { HostName = _hostname };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        Console.WriteLine(channel.ChannelNumber);

        channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var message = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);
        Console.WriteLine("You sent: {0}", message);
    }

    public void AddNewSales(SalesNotificationPublisher publisher)
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
    }

    public void AdjustSales(SalesNotificationPublisher publisher)
    {
        Console.WriteLine("Product type (0-Apple; 1-Banana): ");
        var product = Console.ReadLine();
        var productType = product switch
        {
            "0" => ProductType.Apple,
            "1" => ProductType.Banana,
            _ => ProductType.Apple
        };

        Console.WriteLine("Adjustment operation (+/-/*): ");
        var operation = Console.ReadLine();
        var operationType = operation switch
        {
            "+" => AdjustmentOperation.Add,
            "-" => AdjustmentOperation.Subtract,
            "*" => AdjustmentOperation.Multiply,
            _ => AdjustmentOperation.Add
        };

        Console.WriteLine("Adjustment amount: ");
        var price = decimal.Parse(Console.ReadLine());

        publisher.SendNotification(new SalesNotification
        {
            Product = productType,
            AdjustmentOperation = operationType,
            Price = price
        });
    }
}
