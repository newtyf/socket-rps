using SocketRps.Hubs;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().WithMethods("GET", "POST").AllowCredentials();

    });
});

// Socket
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.MapGet("/", () => "Hello World!");
app.MapHub<GameHub>("/gamehub");

app.Run();