using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; } = null!;
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = null!;
    public string DisplayName { get; set; } = "User";
    public string Biography { get; set; } = "Biography";
}