using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class LoginUserDto
{
    [Required]
    [DisplayName("Username")]
    public string Username { get; set; } = null!;
    [Required]
    [PasswordPropertyText]
    [DisplayName("Password")]
    public string Password { get; set; } = null!;

}