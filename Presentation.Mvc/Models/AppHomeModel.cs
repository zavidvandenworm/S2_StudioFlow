using Domain.Entities;

namespace Presentation.Mvc.Models;

public class AppHomeModel
{
    public string Username { get; set; } = null!;
    public List<Project> Projects { get; set; } = null!;
}