using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Dashboard.SignalR
{
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);

        private readonly IHubContext<NotificationHub, IReceiveNotification> _context;

        public ServerTimeNotifier(IHubContext<NotificationHub, IReceiveNotification> hubContext)
        {
            _context = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var dateTime = DateTime.Now;

                await _context.Clients.All.ReceiveNotification($"Server Time: {dateTime}");
            }
        }
    }
}
