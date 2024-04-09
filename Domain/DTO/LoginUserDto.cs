using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class LoginUserDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = null!;

}