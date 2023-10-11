using Newtonsoft.Json;

namespace Finances.BusinessLayer.Models.YooKassa;

public class Amount
{
    [JsonProperty("value")]
    public string? Value { get; set; }
    [JsonProperty("currency")]
    public string? Currency { get; set; }
}