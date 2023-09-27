namespace Authentication.DomainLayer.Shared.Events
{
    public class EmailModel
    {
        public string Subject { get; set; } = "Subject";
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
