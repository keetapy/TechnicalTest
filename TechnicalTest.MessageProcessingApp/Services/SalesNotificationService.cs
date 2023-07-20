using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TechnicalTest.MessageProcessingApp.Models;
using TechnicalTest.MessageProcessingApp.Services;

namespace Services;

public class SalesNotificationService
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly SalesService _salesService;
    private IModel _channel;
    private int MessagesCount = 0;

    public SalesNotificationService(string hostname, string queueName)
    {
        _hostname = hostname;
        _queueName = queueName;
        _salesService = new SalesService();
    }

    public void StartConsuming()
    {
        using var connection = CreateConnection();
        DeclareQueue(connection);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += HandleReceivedMessage;

        _channel.BasicConsume(queue: _queueName,
                              autoAck: false,
                              consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory() { HostName = _hostname };
        return factory.CreateConnection();
    }

    private void DeclareQueue(IConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    private void HandleReceivedMessage(object? sender, BasicDeliverEventArgs ea)
    {
        var consumingLimit = 50;

        try
        {
            MessagesCount++;

            var message = GetMessageFromDelivery(ea);

            var notification = JsonSerializer.Deserialize<SalesNotification>(message);

            HandleSalesNotification(notification);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);

            if(MessagesCount == consumingLimit)
            {
                StopConsuming();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Message was rejected [{0}]", ex.ToString());
            _channel.BasicReject(ea.DeliveryTag, requeue: true);
        }
    }

    private string GetMessageFromDelivery(BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        return Encoding.UTF8.GetString(body);
    }

    private void HandleSalesNotification(SalesNotification? notification)
    {
        if (notification?.Product is null || notification.Price <= 0)
        {
            Console.WriteLine("SalesNotification is invalid!");
            return;
        }

        var analyticsThreshold = 3;
        var result = _salesService.AddSales(notification)
                ? "Sales were added!"
                : "Adding data failed!";

        Console.WriteLine(result);

        if (MessagesCount % analyticsThreshold == 0)
        {
            foreach (var item in _salesService.GetAnalytics())
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    private void StopConsuming()
    {
        Console.WriteLine("Application Stop consuming");
        _channel.Close();
        _channel.Dispose();
    }
}
