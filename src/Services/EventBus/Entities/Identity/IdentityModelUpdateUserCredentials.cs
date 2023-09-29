namespace EventBus.Entities.Identity
{
    public class IdentityModelUpdateUserCredentials
    {
        public Guid Id { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
    }
}
