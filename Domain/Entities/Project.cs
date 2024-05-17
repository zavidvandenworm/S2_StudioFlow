using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<ProjectTask> Tasks { get; set; } = null!;
    public IEnumerable<ProjectMember> ProjectMembers { get; set; } = null!;
    public IEnumerable<Tag> Tags { get; set; } = null!;
    public IEnumerable<ProjectFile> Files { get; set; } = null!;
    public DateTime Created { get; set; }
}