using Dashboard_Backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace Dashboard.SignalR
{
    public class SalesHub : Hub<IReceiveSalesInfo>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.ReceiveSalesData(new SalesData { Id = 0, Producto = "Conexión establecida", Cantidad = 0, Precio = 0, Fecha = DateTime.Now });
        }

        public async Task SendSalesData(SalesData salesData)
        {
            await Clients.All.ReceiveSalesData(salesData);
        }
    }

    public interface IReceiveSalesInfo
    {
        Task ReceiveSalesData(SalesData salesData);
    }
}
