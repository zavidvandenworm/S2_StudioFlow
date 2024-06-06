namespace EWSDomain.Entities;

public class ProjectFile
{
    public int Id { get; set; }
    public string FileLocation { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public int Version { get; set; }
    public DateTime Created { get; set; }
    
    public Project Project { get; set; } = null!;
    public int ProjectId { get; set; }
}