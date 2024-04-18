using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class CreateProjectDto
{
    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = "Description";
    public int UserId { get; set; }
}