using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TechnicalTest.MessageProcessingApp.Models;
using Microsoft.Extensions.Configuration;
using TechnicalTest.MessageProcessingApp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Services;

public class SalesNotificationService: ISalesNotificationService
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly ISalesService _salesService;
    private IModel _channel;
    private readonly ILogger<SalesNotificationService> _logger;
    private int MessagesCount = 0;

    public SalesNotificationService(
        IConfiguration configuration, 
        ISalesService salesService, 
        ILogger<SalesNotificationService> logger)
    {
        var hostname = configuration.GetSection("RabbitMq")["HostName"];
        var queueName = configuration.GetSection("RabbitMq")["QueueName"];
        _hostname = hostname;
        _queueName = queueName;
        _salesService = salesService;
        _logger = logger;
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
        _logger.LogInformation("Consuming started...");
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
            _logger.LogError("Message was rejected [{0}]", ex.ToString());
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
            _logger.LogWarning("SalesNotification is invalid!");
            return;
        }

        var analyticsThreshold = 10;

        if (notification?.AdjustmentOperation is not null)
        {
            _salesService.AdjustSales(
                notification.Product, 
                notification.AdjustmentOperation, 
                notification.Price);
        }
        else
        {
            var result = _salesService.AddSales(notification)
                ? "Sales were added!"
                : "Adding data failed!";

            _logger.LogInformation(result);
        }
        

        if (MessagesCount % analyticsThreshold == 0)
        {
            var analyticsResult = new StringBuilder();
            foreach (var item in _salesService.GetAnalytics())
            {
               analyticsResult.Append(item.ToString()+"\n\t");
            }

            _logger.LogInformation(analyticsResult.ToString());
        }
    }

    private void StopConsuming()
    {
        _logger.LogInformation("Application stop consuming");
        _channel.Close();
        _channel.Dispose();
    }
}
