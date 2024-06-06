using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationEF.Dtos;

public class RegisterDto
{
    [MaxLength(50)]
    [RegularExpression("^[a-zA-Z0-9\\-\\.]*$")]
    public string Username { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    public string Password { get; set; } = null!;
}