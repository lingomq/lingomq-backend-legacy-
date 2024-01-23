namespace Email.Application.Models
{
    public class EmailTemplate
    {
        public string? Header { get; set; } = "Untitled";
        public string? Text { get; set; } = "No content";
        public string? ButtonName { get; set; } = "<span></span>";
        public string? ButtonHref { get; set; } = "";
    }
}
