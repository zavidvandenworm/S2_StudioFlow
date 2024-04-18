using System.Security.Claims;
using Domain.Entities;

namespace Presentation.Mvc.Helpers;

public static class GenerateAuthClaims
{
    public static List<Claim> GenerateClaims(this User user)
    {
        return [
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Sid, user.Id.ToString())
        ];
    }
}