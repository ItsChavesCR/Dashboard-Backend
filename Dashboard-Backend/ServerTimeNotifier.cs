using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Dashboard_Backend.Models;

namespace Dashboard.SignalR
{
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);

        private readonly IHubContext<SalesHub, IReceiveSalesInfo> _context;

        public ServerTimeNotifier(IHubContext<SalesHub, IReceiveSalesInfo> hubContext)
        {
            _context = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var salesData = new SalesData
                {
                    Id = new Random().Next(1, 1000),
                    Producto = "Producto de prueba",
                    Cantidad = new Random().Next(1, 20),
                    Precio = Math.Round((decimal)(new Random().NextDouble() * 1000), 2),
                    Fecha = DateTime.Now
                };

                await _context.Clients.All.ReceiveSalesData(salesData);
            }
        }
    }
}
