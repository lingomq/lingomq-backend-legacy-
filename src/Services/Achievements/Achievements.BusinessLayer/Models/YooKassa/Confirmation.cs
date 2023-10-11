using Newtonsoft.Json;

namespace Achievements.BusinessLayer.Models.YooKassa;

public class Confirmation
{
    [JsonProperty("type")]
    public string? Type { get; set; }
    [JsonProperty("confirmation_url")]
    public string? ConfirmationUrl { get; set; }
}