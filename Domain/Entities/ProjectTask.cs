using Domain.Common;

namespace Domain.Entities;

public class ProjectTask : BaseEntity
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public IEnumerable<User> Members { get; set; } = null!;
}