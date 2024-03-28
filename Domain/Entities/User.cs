using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required Profile Profile { get; set; }
}