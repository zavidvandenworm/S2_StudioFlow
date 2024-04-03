namespace Infrastructure.DTO;

public class CreateUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
    public required string Biography { get; set; }
}