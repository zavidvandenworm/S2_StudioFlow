namespace Domain.DTO;

public class CreateFileDto
{
    public int ProjectId { get; set; }
    public string FileName { get; set; } = null!;
    public byte[] Content { get; set; } = null!;
}