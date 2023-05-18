using System.Diagnostics;
using Newtonsoft.Json;
using SocketRps;
using SocketRps.Hubs;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").WithOrigins("https://rps.newty.com").AllowAnyHeader().WithMethods("GET", "POST").AllowCredentials();

    });
});

// Socket
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(myAllowSpecificOrigins);
app.MapGet("/", () => "Hello World");
app.MapHub<GameHub>("/gamehub");

var PORT = Environment.GetEnvironmentVariable("PORT") ?? "5000";

app.Run($"https://localhost:{PORT}");