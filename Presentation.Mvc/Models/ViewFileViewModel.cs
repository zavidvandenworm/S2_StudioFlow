using Domain.Entities;

namespace Presentation.Mvc.Models;

public class ViewFileViewModel
{
    public ProjectFile ProjectFile { get; set; } = null!;
    public int ProjectId { get; set; }
    public List<ProjectFile> Versions { get; set; } = [];
}