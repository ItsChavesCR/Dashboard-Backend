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
    private ServiceBusProcessor _processor;

    public ServiceBusProcessorService(IOptions<ServiceBusSettings> serviceBusSettings)
    {
        var settings = serviceBusSettings.Value;
        _client = new ServiceBusClient(settings.ConnectionString);
        _queueName = settings.QueueName;
    }

    public async Task StartProcessingMessagesAsync(CancellationToken cancellationToken)
    {
        _processor = _client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(cancellationToken);
        Console.WriteLine("Esperando mensajes...");

        cancellationToken.Register(async () => await StopProcessingMessagesAsync());
    }

    private async Task StopProcessingMessagesAsync()
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
            Console.WriteLine("Procesador de mensajes detenido.");
        }
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
        Console.WriteLine($"Error al procesar mensaje: {args.Exception.Message}");
        Console.WriteLine($"Contexto del error: {args.ErrorSource}");
        Console.WriteLine($"Entidad del error: {args.EntityPath}");
        return Task.CompletedTask;
    }
}
