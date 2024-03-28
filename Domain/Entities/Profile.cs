using Domain.Common;

namespace Domain.Entities;

public class Profile : BaseEntity
{
    public required string DisplayName { get; set; }
    public required string Biography { get; set; }
}