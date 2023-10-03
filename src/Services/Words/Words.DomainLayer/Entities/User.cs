namespace Words.DomainLayer.Entities
{
    public class User : BaseEntity
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
    }
}
