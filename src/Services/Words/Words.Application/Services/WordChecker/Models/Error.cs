using Newtonsoft.Json;

namespace Words.Application.Services.WordChecker.Models;

public class Error
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("offset")]
    public int Offset { get; set; }
    [JsonProperty("length")]
    public int Length { get; set; }
    [JsonProperty("bad")]
    public string? Bad { get; set; }
    [JsonProperty("better")]
    public ICollection<string>? Better { get; set; }
}
