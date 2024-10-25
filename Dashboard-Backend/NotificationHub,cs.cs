using Microsoft.AspNetCore.SignalR;

namespace Dashboard_Backend.Hubs
{
    public class NotificationHub : Hub<IReceiveNotification>
    {
        // Método que se ejecuta cuando un cliente se conecta al NotificationHub
        public override async Task OnConnectedAsync()
        {
            // Envía una notificación al cliente que se acaba de conectar
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"Gracias, conectando {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }
    }

    // Interfaz para definir los métodos que los clientes pueden recibir
    public interface IReceiveNotification
    {
        Task ReceiveNotification(string message);
    }
}
