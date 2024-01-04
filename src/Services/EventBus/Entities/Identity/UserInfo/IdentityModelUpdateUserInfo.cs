namespace EventBus.Entities.Identity.UserInfo
{
    public class IdentityModelUpdateUserInfo
    {
        public Guid Id { get; set; }
        public string? Nickname { get; set; }
        public string? ImageUri { get; set; }
        public string? Additional { get; set; }
        public Guid UserLinkId { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreationalDate { get; set; }
        public bool IsRemoved { get; set; } = false;
    }
}
