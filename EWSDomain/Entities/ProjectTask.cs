using EWSDomain.Common;

namespace EWSDomain.Entities;

public class ProjectTask : BaseEntity
{
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public List<ProjectFile> ProjectFiles { get; set; } = null!;
    public List<ProjectMember> Members { get; set; } = null!;
    public bool Completed { get; set; }
}