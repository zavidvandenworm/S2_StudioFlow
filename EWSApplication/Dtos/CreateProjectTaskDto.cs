using System.ComponentModel.DataAnnotations;

namespace ApplicationEF.Dtos;

public class CreateProjectTaskDto
{
    public int ProjectId { get; set; }
    [MaxLength(200)]
    public string Name { get; set; } = null!;
    [MaxLength(5000)]
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    [MaxLength(100)]
    public IEnumerable<ProjectMemberReferenceDto> Members { get; set; } = null!;

    public IEnumerable<ProjectFileReferenceDto> Files { get; set; } = null!;
}