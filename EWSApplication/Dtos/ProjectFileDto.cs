namespace ApplicationEF.Dtos;

public class ProjectFileDto
{
    public string FileLocation { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public int Version { get; set; }
    public DateTime Created { get; set; }
}