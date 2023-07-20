using Microsoft.Extensions.Configuration;
using Services;
using TechnicalTest.MessageProcessingApp;

IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .Build();

var hostname = configuration.GetSection("RabbitMq")["HostName"];
var queueName = configuration.GetSection("RabbitMq")["QueueName"];

var consumer = new SalesNotificationService(hostname, queueName);
consumer.StartConsuming();
Console.ReadKey();