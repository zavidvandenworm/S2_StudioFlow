using Domain.Enums;

namespace Domain.Entities;

public class AuthState
{
    public required User User { get; init; }
    public DateTimeOffset AuthExpire = DateTimeOffset.Now.AddHours(12);
}