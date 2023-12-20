namespace Achievements.Domain.Entities;
public class Achievement : EntityBase
{
    public string? Name { get; set; }
    public string? Content { get; set; }
    public string? ImageUri { get; set; }
}
