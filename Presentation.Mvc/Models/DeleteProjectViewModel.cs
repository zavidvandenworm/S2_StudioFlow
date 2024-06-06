using Domain.Entities;

namespace Presentation.Mvc.Models;

public class DeleteProjectViewModel
{
    public Project Project { get; set; } = null!;
    public string ProjectNameVerification { get; set; } = null!;
}