namespace ApplicationEF.Dtos;

public class UpdateProjectTaskDto
{
    public int TaskId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public List<ProjectFileReferenceDto> ProjectFiles { get; set; } = null!;
    public List<ProjectMemberReferenceDto> Members { get; set; } = null!;
}