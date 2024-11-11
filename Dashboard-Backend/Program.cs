using Dashboard.SignalR;
using Dashboard_Backend.Models;
using Dashboard_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Origen del frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Permitir credenciales si estás usando autenticación con cookies o SignalR
    });
});

// Agregar servicios de SignalR y controladores
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));
builder.Services.AddSignalR();
builder.Services.AddHostedService<ServerTimeNotifier>();
builder.Services.AddControllers();
builder.Services.AddSingleton<TokenService>();

// Configuración de Swagger con autenticación JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' seguido de su token JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

//var serviceBusProcessor = app.Services.GetRequiredService<ServiceBusProcessorService>();
//var cts = new CancellationTokenSource();
//await serviceBusProcessor.StartProcessingMessagesAsync(cts.Token);

app.UseHttpsRedirection();

// Habilita autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Usa la política CORS
app.UseCors("AllowFrontend");

// Configura Swagger
app.UseSwagger();
app.UseSwaggerUI();



// Mapea hubs de SignalR
app.MapHub<SalesHub>("/hub/Saleshub");
app.MapHub<NotificationHub>("/hub/notificationhub"); // Corrige aquí el nombre del endpoint si era incorrecto

// Mapea controladores
app.MapControllers();

app.Run();
