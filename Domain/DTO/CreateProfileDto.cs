namespace Infrastructure.DTO;

public class CreateProfileDto
{
    public required int UserId { get; set; }
    public required string DisplayName { get; set; }
    public required string Biography { get; set; }
}