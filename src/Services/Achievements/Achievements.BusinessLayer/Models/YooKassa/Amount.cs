using Newtonsoft.Json;

namespace Achievements.BusinessLayer.Models.YooKassa;

public class Amount
{
    [JsonProperty("value")]
    public string? Value { get; set; }
    [JsonProperty("currency")]
    public string? Currency { get; set; }
}