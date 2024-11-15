using Azure.Messaging.ServiceBus;
using Dashboard_Backend.Models;
using Microsoft.AspNetCore.SignalR;
using Dashboard.SignalR;
using System.Text.Json;

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
            try
            {
                // Deserialize message body
                var body = args.Message.Body.ToObjectFromJson<SimplifiedSalesData>();

                // Transform into simplified model
                var simplifiedData = new SimplifiedSalesData
                {
                    Name = body.Name,
                    Description = body.Description,
                    Price = body.Price,
                    AffiliateId = body.AffiliateId
                };

                // Broadcast simplified data to SignalR clients
                await _hubContext.Clients.All.ReceiveSalesData(JsonSerializer.Serialize(simplifiedData));

                // Complete the message so it is not received again
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                // Optionally: Dead-letter the message or take corrective action
            }
        };

        processor.ProcessErrorAsync += args =>
        {
            Console.WriteLine($"Error: {args.Exception.Message}");
            return Task.CompletedTask;
        };

        await processor.StartProcessingAsync();

        // Keep the processor running until the background service is stopped
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}