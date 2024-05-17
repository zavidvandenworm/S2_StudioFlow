using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class CreateUserDto
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$")]
    public string Username { get; set; } = null!;
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    [PasswordPropertyText]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{7,99}$")]
    [MinLength(7)]
    public string Password { get; set; } = null!;
    public string DisplayName { get; set; } = "User";
    public string Biography { get; set; } = "Biography";
}