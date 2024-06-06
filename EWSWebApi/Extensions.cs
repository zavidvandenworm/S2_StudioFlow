using System.Security.Claims;

namespace EWSWebApi;

public static class Extensions
{
    public static int? GetUserId(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (claim is null)
        {
            return null;
        }
        else
        {
            return Convert.ToInt32(claim.Value);
        }
    }
}