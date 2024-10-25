using Microsoft.AspNetCore.SignalR;
using Dashboard_Backend.Hubs;

namespace Dashboard_Backend.Services
{
    // Servicio para notificar actualizaciones en tiempo real desde el servidor
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);

        // Contexto del Hub para enviar notificaciones a los clientes conectados
        private readonly IHubContext<DashboardHub, IDashboardClient> _hubContext;

        public ServerTimeNotifier(IHubContext<DashboardHub, IDashboardClient> hubContext)
        {
            _hubContext = hubContext;
        }

        // Método que se ejecuta continuamente en segundo plano
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var dateTime = DateTime.Now;

                // Aquí se envía la actualización en tiempo real a todos los clientes conectados
                await _hubContext.Clients.All.ReceiveUpdate($"Actualización del servidor: {dateTime}");
            }
        }
    }
}
