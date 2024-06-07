namespace ApplicationEF.Dtos;

public class ProjectTaskDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public IEnumerable<ProjectMemberReferenceDto> Members { get; set; } = null!;
    public bool Completed { get; set; }
}