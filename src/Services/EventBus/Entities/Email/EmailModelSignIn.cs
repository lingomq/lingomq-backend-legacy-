namespace EventBus.Entities.Email
{
    public class EmailModelSignIn
    {
        public string Subject { get; set; } = "Subject";
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
