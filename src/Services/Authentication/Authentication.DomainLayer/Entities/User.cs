using System.ComponentModel;

namespace Authentication.DomainLayer.Entities
{
    public class User : BaseEntity
    {
        public string? Phone { get; set; }
        [Description("email")]
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
    }
}
