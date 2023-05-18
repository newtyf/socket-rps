using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SocketRps.Hubs;

public class GameHub: Hub
{
    private HttpClient _client = new HttpClient();
    private readonly IConfiguration _configuration;

    public GameHub(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task PickGameOption(string room, string name, string pick)
    {
        await Clients.GroupExcept(room, excludedConnectionId1: Context.ConnectionId).SendAsync("pickGame", name);
    }
    
    public async Task JoinGroup(string id, string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.GroupExcept(room, excludedConnectionId1: Context.ConnectionId).SendAsync("joinPlayer", id);
    }

    public async Task ExitGroup(string name, string room)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        await Clients.GroupExcept(room, excludedConnectionId1: Context.ConnectionId).SendAsync("exitPlayer", $"{name} salio a la sala");
    }

    public async Task GetWinner(string roomId)
    {
        try
        {
            User? playerOne;
            User? playerTwo;
            
            HttpResponseMessage responseMessage = await _client.GetAsync($"{_configuration["ApiUrlString:api"]}/rooms/{roomId}");
            
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            Debug.WriteLine($"aaaaaaaa, {responseBody}");
            
            Room? room = JsonConvert.DeserializeObject<Room>(responseBody);
            Debug.WriteLine($"aaaaaaaa, {room._id}");

            HttpResponseMessage responseOne =
                await _client.GetAsync($"{_configuration["ApiUrlString:api"]}/users/{room.users[0]}");
            string responseBodyOne = await responseOne.Content.ReadAsStringAsync();
            playerOne = JsonConvert.DeserializeObject<User>(responseBodyOne);
            
            HttpResponseMessage responseTwo =
                await _client.GetAsync($"{_configuration["ApiUrlString:api"]}/users/{room.users[1]}");
            string responseBodyTwo = await responseTwo.Content.ReadAsStringAsync();
            playerTwo = JsonConvert.DeserializeObject<User>(responseBodyTwo);
            
            Debug.WriteLine(playerOne.Pick);
            Debug.WriteLine( playerTwo.Pick);
            
            Debug.WriteLine(playerOne.Pick != null && playerTwo.Pick != null);

            if (playerOne.Pick != null && playerTwo.Pick != null)
            {
                if (
                    (playerOne.Pick.Equals("PAPER") && playerTwo.Pick.Equals("ROCK")) ||
                    (playerOne.Pick.Equals("ROCK") && playerTwo.Pick.Equals("SCISSOR")) ||
                    (playerOne.Pick.Equals("SCISSOR") && playerTwo.Pick.Equals("PAPER"))
                ) {
                    Debug.WriteLine("entro a escoger el ganador");
                    await Clients.Group(roomId).SendAsync("resultGame", JsonConvert.SerializeObject(playerOne));
                } else if (playerOne.Pick.Equals(playerTwo.Pick)) {
                    Debug.WriteLine("entro a escoger el ganador");
                    await Clients.Group(roomId).SendAsync("resultGame", "Draw");
                } else {
                    Debug.WriteLine("entro a escoger el ganador");
                    await Clients.Group(roomId).SendAsync("resultGame", JsonConvert.SerializeObject(playerTwo));
                }
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }
}