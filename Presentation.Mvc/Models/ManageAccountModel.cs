using Domain.Entities;

namespace Presentation.Mvc.Models;

public class ManageAccountModel
{
    public User User { get; set; } = null!;
    public Profile Profile { get; set; } = null!;
}