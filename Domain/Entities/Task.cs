using Domain.Common;

namespace Domain.Entities;

public class Task : BaseEntity
{
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public IEnumerable<User> Members { get; set; } = null!;
}