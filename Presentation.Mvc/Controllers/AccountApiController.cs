using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.Users.Commands.UpdateProfile;
using Application.Users.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Presentation.Mvc.Controllers;

[Authorize]
public class AccountApiController : Controller
{
    private readonly ISender _sender;

    public AccountApiController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> ApiChangeDisplayName([MinLength(1)]string newDisplayName)
    {
        var userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        var profile = await _sender.Send(new GetProfileQuery
            { UserId = userId });
        if (profile.DisplayName == newDisplayName)
        {
            return Content(JsonSerializer.Serialize(new
            {
                Status = "error",
                Message = "New display name is equivelant to old name."
            }));
        }
        profile.DisplayName = newDisplayName;
        try
        {
            await _sender.Send(new UpdateProfileCommand { UserId = userId, Profile = profile });
            return Content(JsonConvert.SerializeObject(new
            {
                Status = "success",
                Message = "Username has been updated."
            }));
        }
        catch (Exception)
        {
            return Content(JsonConvert.SerializeObject(new
            {
                Status = "error",
                Message = "An error occured while updating the username."
            }));
        }
    }
}