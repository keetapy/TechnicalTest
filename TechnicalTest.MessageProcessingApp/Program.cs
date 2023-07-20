using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using TechnicalTest.MessageProcessingApp.Repositories;
using TechnicalTest.MessageProcessingApp.Repositories.Interfaces;
using TechnicalTest.MessageProcessingApp.Services;
using TechnicalTest.MessageProcessingApp.Services.Interfaces;

var builder = new HostBuilder().ConfigureAppConfiguration(configuration =>
{
    configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true);
}).ConfigureServices((hostContext, service) =>
{
    service
        .AddScoped<ISalesRepository, SalesRepository>()
        .AddScoped<ISalesNotificationService, SalesNotificationService>()
        .AddScoped<ISalesService, SalesService>();
});
using IHost host = builder.Build();

var salesNotificationService = host.Services.GetRequiredService<ISalesNotificationService>();
salesNotificationService.StartConsuming();

host.Run();