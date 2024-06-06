using Domain.DTO;

namespace Presentation.Mvc.Models;

public class CreateTaskViewModel
{
    public int ProjectId { get; set; }
    public CreateTaskDto CreateTaskDto { get; set; } = null!;
    public string TagsSeperated { get; set; } = null!;
    
}