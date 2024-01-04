namespace EventBus.Entities.Identity.User
{
    public class IdentityModelUpdateUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; } = "";
    }
}
