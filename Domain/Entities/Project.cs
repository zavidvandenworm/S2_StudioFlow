using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required IEnumerable<Task> Tasks { get; set; }
    public required IEnumerable<ProjectMember> ProjectMembers { get; set; }
}