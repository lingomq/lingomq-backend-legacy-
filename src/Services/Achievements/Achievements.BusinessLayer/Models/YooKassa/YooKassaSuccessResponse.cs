using Newtonsoft.Json;

namespace Achievements.BusinessLayer.Models.YooKassa;

public class YooKassaSuccessResponse
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("status")]
    public string? Status { get; set; }
    [JsonProperty("paid")]
    public bool Paid { get; set; }
    [JsonProperty("amount")]
    public Amount? Amount { get; set; }
    [JsonProperty("confirmation")]
    public Confirmation? Confirmation { get; set; }
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }
    [JsonProperty("metadata")]
    public Metadata? Metadata { get; set; }
    [JsonProperty("refundable")]
    public bool Refundable { get; set; }
    [JsonProperty("test")]
    public bool Test { get; set; }
}