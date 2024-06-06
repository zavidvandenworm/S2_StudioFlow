using EWSDomain.Entities;

namespace ApplicationEF.Dtos;

public class ProjectDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ProjectTaskDto> Tasks { get; set; } = null!;
    public List<ProjectMemberDto> ProjectMembers { get; set; } = null!;
    public List<ProjectDto> Files { get; set; } = null!;
    public DateTime Created { get; set; }
}