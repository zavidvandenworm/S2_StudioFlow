using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ProjectMember : BaseEntity
{
    public required User User { get; set; }
    public required ProjectRole ProjectRole { get; set; }
}