namespace Domain.Entities;

public class ProjectFile
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public byte[] FileContents { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public int Version { get; set; }
    public DateTime Created { get; set; }
}