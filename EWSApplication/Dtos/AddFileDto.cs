namespace ApplicationEF.Dtos;

public class AddFileDto
{
    public string NewFileName { get; set; } = null!;
    public int ProjectId { get; set; }
}