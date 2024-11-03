using Dashboard.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de SignalR, controladores, y el servicio de notificación
builder.Services.AddSignalR();
builder.Services.AddHostedService<ServerTimeNotifier>();
builder.Services.AddCors();
builder.Services.AddControllers(); // Agregar servicios para controladores

// Agregar Swagger para la documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración para el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear los hubs de SignalR
app.MapHub<ChatHub>("/Chat-hub");
app.MapHub<NotificationHub>("/notifications");

// Mapear controladores, incluido el WeatherForecastController
app.MapControllers();

app.Run();
