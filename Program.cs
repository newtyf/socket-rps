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
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().WithMethods("GET", "POST").AllowCredentials();

    });
});

// Socket
builder.Services.AddSignalR();

var app = builder.Build();



app.UseCors(myAllowSpecificOrigins);
app.MapGet("/", async () =>
{
    HttpClient _client = new HttpClient();
    HttpResponseMessage responseMessage = await _client.GetAsync($"http://localhost:3002/api/rooms/64655a1c8a3430edcd9a0e49");
            
    string responseBody = await responseMessage.Content.ReadAsStringAsync();
    Debug.WriteLine($"aaaaaaaa, {responseBody}");
    
    Room room = JsonConvert.DeserializeObject<Room>(responseBody);
    Debug.WriteLine($"aaaassssaaaa, {room._id}");
});

app.MapHub<GameHub>("/gamehub");

app.Run();