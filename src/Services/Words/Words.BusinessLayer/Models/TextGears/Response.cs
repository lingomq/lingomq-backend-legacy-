using Newtonsoft.Json;

namespace Words.BusinessLayer.Models.TextGears
{
    public class Response
    {
        [JsonProperty("result")]
        public bool Result { get; set; }
        [JsonProperty("errors")]
        public ICollection<Error>? Errors { get; set; }
    }
}
