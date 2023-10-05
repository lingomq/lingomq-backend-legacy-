using Newtonsoft.Json;

namespace Words.BusinessLayer.Models.TextGears
{
    public class TextGearsResponseModel
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("response")]
        public Response? Response { get; set; }
    }
}
