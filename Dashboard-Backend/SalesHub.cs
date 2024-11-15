using Dashboard_Backend.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Dashboard.SignalR
{
    public class SalesHub : Hub<IReceiveSalesInfo>
    {
        public async Task SendSalesData(SimplifiedSalesData simplifiedsalesData)
        {
            // Enviar los datos simplificados directamente
            await Clients.All.ReceiveSalesData(JsonSerializer.Serialize(simplifiedsalesData));
        }
    }

    public interface IReceiveSalesInfo
    {
        Task ReceiveSalesData(string salesDataJson);
    }
}
