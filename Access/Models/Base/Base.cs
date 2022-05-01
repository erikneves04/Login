using Access.Interfaces.Models;

namespace Access.Models.Base;

public abstract class Base : IBaseModel
{
    public Base()
    {
        Id = Guid.NewGuid();

        CreatedAt = DateTime.Now;
        LastUpdate = DateTime.MinValue;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
}
