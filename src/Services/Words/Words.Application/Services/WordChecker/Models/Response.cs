using Newtonsoft.Json;

namespace Words.Application.Services.WordChecker.Models;

public class Response
{
    [JsonProperty("result")]
    public bool Result { get; set; }
    [JsonProperty("errors")]
    public ICollection<Error>? Errors { get; set; }
}
