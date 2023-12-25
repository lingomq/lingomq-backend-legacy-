using Newtonsoft.Json;

namespace Words.Application.Services.WordChecker.Models;

public class TextGearsResponseModel
{
    [JsonProperty("status")]
    public bool Status { get; set; }
    [JsonProperty("response")]
    public Response? Response { get; set; }
}
