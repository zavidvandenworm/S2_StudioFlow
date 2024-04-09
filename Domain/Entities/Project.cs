using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<Task> Tasks { get; set; } = null!;
    public IEnumerable<ProjectMember> ProjectMembers { get; set; } = null!;
}