namespace ApplicationEF.Dtos;

public class AiCompleteDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> Tags { get; set; } = null!;
}