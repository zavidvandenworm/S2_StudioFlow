using System.ComponentModel.DataAnnotations;
using EWSDomain.Entities;

namespace ApplicationEF.Dtos;

public class UpdateProjectDto
{
    public required int ProjectId { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(5000)]
    public string Description { get; set; } = null!;
    
    [MaxLength(200)]
    public List<ProjectMemberDto> ProjectMembers { get; set; } = null!;
    
    [MaxLength(50)]
    public List<string> Tags { get; set; } = null!;
}