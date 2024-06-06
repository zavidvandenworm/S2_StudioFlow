using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.DTO;

public class CreateTaskDto
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<IFile> Attachments { get; set; } = [];
    public TaskCategory Category { get; set; }
    public DateTime Deadline { get; set; }
    public List<ProjectMember> Members { get; set; } = [];
    public List<string> Tags { get; set; } = [];
}