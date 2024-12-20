﻿using Azure.Messaging.ServiceBus;
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
                // Deserialize message body to SalesData
                var body = args.Message.Body.ToObjectFromJson<SalesData>();

                // Mostrar datos completos en la consola
                Console.WriteLine("Mensaje recibido desde Service Bus:");
                Console.WriteLine(JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));

                // Transform to SimplifiedSalesData
                var simplifiedData = body.Products.Select(product => new SimplifiedSalesData
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    AffiliateId = body.AffiliateId,
                    PurchaseDate = body.PurchaseDate,
                    Amount = body.Amount,
                }).ToList();

                // Serializa el objeto SimplifiedSalesData a JSON
                string serializedData = JsonSerializer.Serialize(simplifiedData);

                // Enviar los datos serializados a través de SignalR
                await _hubContext.Clients.All.ReceiveSalesData(serializedData);


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
