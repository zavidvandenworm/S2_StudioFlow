using System.ComponentModel.DataAnnotations;
using EWSDomain.Common;

namespace EWSDomain.Entities;

public class Profile : BaseEntity
{
    [MaxLength(50)]
    public string DisplayName { get; set; } = null!;
    [MaxLength(5000)]
    public string Biography { get; set; } = null!;
    [MaxLength(100)]
    public string ProfilePicture { get; set; } = null!;
}