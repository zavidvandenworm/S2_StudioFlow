using System.ComponentModel.DataAnnotations;

namespace ApplicationEF.Dtos;

public class CreateProjectDto
{
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [MaxLength(5000)]
    public string Description { get; set; } = null!;
    [MaxLength(200)]
    public List<ProjectMemberDto> ProjectMembers { get; set; } = null!;
    [MaxLength(50)]
    public List<string> Tags { get; set; } = null!;
}