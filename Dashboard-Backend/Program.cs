using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dashboard_Backend.Services;
using Dashboard_Backend.Hubs;
using Dashboard_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios necesarios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); // Agregar SignalR
builder.Services.AddHostedService<ServerTimeNotifier>(); // Servicio de notificaciones en tiempo real
builder.Services.AddCors(); // Agregar CORS para permitir conexiones externas

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()    // Permite todas las URLs
              .AllowAnyHeader()    // Permite cualquier encabezado
              .AllowAnyMethod();   // Permite cualquier método (GET, POST, etc.)
    });
});
// Configurar JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Agregar servicio de Token
builder.Services.AddScoped<TokenService>();

// SignalR
builder.Services.AddSignalR();


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
app.UseCors("AllowAllOrigins");
app.UseRouting();


app.MapHub<DashboardHub>("dashboardHub"); // Endpoint para el DashboardHub
app.MapHub<NotificationHub>("/notifications"); // Endpoint para el NotificationHub

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
