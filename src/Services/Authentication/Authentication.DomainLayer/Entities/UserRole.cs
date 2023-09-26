using System.ComponentModel;

namespace Authentication.DomainLayer.Entities
{
    public class UserRole : BaseEntity
    {
        [Description("name")]
        public string? Name { get; set; }
    }
}
