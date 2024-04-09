using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Infrastructure.DTO;

public class CreateProjectDto
{
    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = "Description";
    [Required]
    public int UserId { get; set; }
}