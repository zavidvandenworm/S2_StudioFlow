using EWSDomain.Common;
using EWSDomain.Enums;

namespace EWSDomain.Entities;

public class ProjectMember : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public ProjectRole ProjectRole { get; set; }
}