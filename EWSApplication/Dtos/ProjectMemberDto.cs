using EWSDomain.Enums;

namespace ApplicationEF.Dtos;

public class ProjectMemberDto
{
    public required int UserId { get; set; }
    public required ProjectRole Role { get; set; }
}