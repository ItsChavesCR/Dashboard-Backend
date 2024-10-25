using Microsoft.AspNetCore.SignalR;

namespace Dashboard_Backend.Hubs
{
    // El Hub que gestiona las actualizaciones del dashboard
    public class DashboardHub : Hub<IDashboardClient>
    {
        // Método que se ejecuta cuando un cliente se conecta al DashboardHub
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveUpdate($"Conectado al dashboard con ID: {Context.ConnectionId}");
        }

        // Método para enviar actualizaciones desde el backend
        public async Task SendUpdate(string update)
        {
            await Clients.All.ReceiveUpdate($"{Context.ConnectionId}: {update}");
        }
    }

    // Interfaz para definir los métodos que los clientes pueden recibir
    public interface IDashboardClient
    {
        Task ReceiveUpdate(string update);
    }
}
