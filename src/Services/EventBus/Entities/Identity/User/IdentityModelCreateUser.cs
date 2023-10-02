namespace EventBus.Entities.Identity.User
{
    public class IdentityModelCreateUser
    {
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; } = "";
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? RoleName { get; set; }
    }
}
