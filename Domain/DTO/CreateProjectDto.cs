using Domain.Entities;

namespace Infrastructure.DTO;

public class CreateProjectDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required User Creator { get; set; }
}