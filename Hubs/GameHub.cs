using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace SocketRps.Hubs;

public class GameHub: Hub
{

    public async Task PickGameOption(string room, string name, string pick)
    {
        await Clients.Group(room).SendAsync("pickGame", name, pick);
    }
    
    public async Task JoinGroup(string id, string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.GroupExcept(room, excludedConnectionId1: Context.ConnectionId).SendAsync("joinPlayer", id);
    }

    public async Task ExitGroup(string name, string room)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        await Clients.GroupExcept(room, excludedConnectionId1: Context.ConnectionId).SendAsync("joinPlayer", $"{name} salio a la sala");
    }
}