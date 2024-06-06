using Domain.Common;

namespace Domain.Entities;

public class Profile : BaseEntity
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public string ProfilePicture { get; set; } = null!;
}