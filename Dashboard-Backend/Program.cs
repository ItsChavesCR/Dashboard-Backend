using Dashboard_Backend.Hubs;
using Dashboard_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios necesarios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); // Agregar SignalR
builder.Services.AddHostedService<ServerTimeNotifier>(); // Servicio de notificaciones en tiempo real
builder.Services.AddCors(); // Agregar CORS para permitir conexiones externas

var app = builder.Build();

// Configurar Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar CORS para permitir cualquier origen, método y header
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();
app.UseRouting();

app.MapHub<DashboardHub>("dashboardHub"); // Endpoint para el DashboardHub
app.MapHub<NotificationHub>("/notifications"); // Endpoint para el NotificationHub

app.Run();
