using System.ComponentModel.DataAnnotations;
using EWSDomain.Common;

namespace EWSDomain.Entities;

public class Project : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [MaxLength(5000)]
    public string Description { get; set; } = null!;
    [MaxLength(500)]
    public List<ProjectTask> Tasks { get; set; } = null!;
    [MaxLength(200)]
    public List<ProjectMember> ProjectMembers { get; set; } = null!;
    [MaxLength(50)]
    public List<string> Tags { get; set; } = null!;
    [MaxLength(100)]
    public List<ProjectFile> Files { get; set; } = null!;
    public DateTime Created { get; set; }
}