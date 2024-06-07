namespace ApplicationEF.Dtos;

public class ProjectFileDto
{
    public string FileName { get; set; } = null!;
    public string FileId { get; set; } = null!;
    public int Version { get; set; }
    public DateTimeOffset Created { get; set; }
}