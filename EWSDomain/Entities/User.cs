using EWSDomain.Common;

namespace EWSDomain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Profile Profile { get; set; } = null!;
}