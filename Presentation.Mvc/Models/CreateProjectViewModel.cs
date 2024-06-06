using Domain.DTO;

namespace Presentation.Mvc.Models;

public class CreateProjectViewModel
{
    public CreateProjectDto CreateProjectDto { get; set; } = null!;
    public string TagsSplit { get; set; } = null!;
}