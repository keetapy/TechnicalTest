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
}
