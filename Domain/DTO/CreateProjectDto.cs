using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTO;

public class CreateProjectDto
{
    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = "Description";
    public List<string> Tags { get; set; } = [];
    public DigitalAudioWorkstation DigitalAudioWorkstation { get; set; }
    public int UserId { get; set; }
}