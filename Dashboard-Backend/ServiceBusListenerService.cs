using Azure.Messaging.ServiceBus;
using Dashboard_Backend.Models;
using Microsoft.AspNetCore.SignalR;
using Dashboard.SignalR;

public class ServiceBusListenerService : BackgroundService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly IHubContext<SalesHub, IReceiveSalesInfo> _hubContext;
    private readonly string _queueName = "dev-event-source-1";

    public ServiceBusListenerService(ServiceBusClient serviceBusClient, IHubContext<SalesHub, IReceiveSalesInfo> hubContext)
    {
        _serviceBusClient = serviceBusClient;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = _serviceBusClient.CreateProcessor(_queueName);

        processor.ProcessMessageAsync += async args =>
        {
            var body = args.Message.Body.ToObjectFromJson<SalesData>();

            // Broadcast to SignalR clients
            await _hubContext.Clients.All.ReceiveSalesData(body);

            // Complete the message so it is not received again
            await args.CompleteMessageAsync(args.Message);
        };

        processor.ProcessErrorAsync += args =>
        {
            Console.WriteLine($"Error: {args.Exception}");
            return Task.CompletedTask;
        };

        await processor.StartProcessingAsync();

        // Keep the processor running until the background service is stopped
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
