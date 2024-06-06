namespace Domain.DTO;

public class CreateProfileDto
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Biography { get; set; } = null!;
}