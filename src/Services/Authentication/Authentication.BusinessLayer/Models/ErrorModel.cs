using Newtonsoft.Json;

namespace Authentication.BusinessLayer.Models
{
    public class ErrorModel
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
        public override string ToString()
        {
            return StatusCode + " " + Message;
        }
    }
}
