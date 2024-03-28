using Domain.Common;

namespace Domain.Entities;

public class Task : BaseEntity
{
    public required string Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required IEnumerable<User> Members { get; set; }
}