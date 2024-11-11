
using Azure.Messaging.ServiceBus;
using Dashboard_Backend.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Dashboard_Backend.Services;
public class ServiceBusProcessorService
{
    private readonly ServiceBusClient _client;
    private readonly string _queueName;

    public ServiceBusProcessorService(IOptions<ServiceBusSettings> serviceBusSettings)
    {
        var settings = serviceBusSettings.Value;
        _client = new ServiceBusClient(settings.ConnectionString);
        _queueName = settings.QueueName;
    }

    public async Task StartProcessingMessagesAsync(CancellationToken cancellationToken)
    {
        var processor = _client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync();
        Console.WriteLine("Esperando mensajes...");

        cancellationToken.Register(async () =>
        {
            await processor.StopProcessingAsync();
            await processor.DisposeAsync();
        });
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine($"Mensaje recibido: {body}");

        // Completar el mensaje para confirmar la recepción y evitar reintentos
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error: {args.Exception.Message}");
        return Task.CompletedTask;
    }
}
