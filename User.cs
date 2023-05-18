using Newtonsoft.Json;

namespace SocketRps;

public class User
{
    [JsonProperty("_id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("room")]
    public Room Room { get; set; }
    
    [JsonProperty("pick")]
    public string Pick { get; set; }
    
    [JsonProperty("points")]
    public int Points { get; set; }
}