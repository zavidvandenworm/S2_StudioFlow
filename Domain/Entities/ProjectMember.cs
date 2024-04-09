using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ProjectMember : BaseEntity
{
    public int UserId { get; set; }
    public ProjectRole ProjectRole { get; set; }
}