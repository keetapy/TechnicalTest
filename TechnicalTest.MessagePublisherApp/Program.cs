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
    switch (usersResult)
    {
        case "0": publisher.AddNewSales(publisher); break;
        case "1": publisher.AdjustSales(publisher); break;
    }

    Console.WriteLine("0 - Add new sales; 1 - AdjustSales");
    Console.WriteLine("(Enter \"stop\" to stop app)");
    usersResult = Console.ReadLine();
}
