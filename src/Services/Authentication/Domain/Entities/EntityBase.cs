namespace Authentication.Domain.Entities;

public class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

