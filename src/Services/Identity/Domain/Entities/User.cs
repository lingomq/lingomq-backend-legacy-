namespace Identity.Domain.Entities;
public class User : EntityBase
{
    public string? Nickname { get; set; }
    public string? ImageUri { get;set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
