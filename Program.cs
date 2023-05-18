using System.Diagnostics;
using Newtonsoft.Json;
using SocketRps;
using SocketRps.Hubs;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional:true, reloadOnChange: true)
    .Build();

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

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
var url = $"{configuration["urlString:url"]}:{port}";

app.Run(url);