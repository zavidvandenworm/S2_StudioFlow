using System.Security.Claims;
using ApplicationEF;
using ApplicationEF.Dtos;
using ApplicationEF.Exceptions;
using ApplicationEF.Users.Commands;
using ApplicationEF.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWSWebApi.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var user = await _sender.Send(new CreateUserCommand
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = registerDto.Password
            });
            var token = GenerateJwt.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
        catch (UserExistsException)
        {
            return Conflict("User with that name already exists.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var user = await _sender.Send(new GetUserFromUsernameQuery
            {
                Username = loginDto.Username
            });
            if (user.PasswordHash != PasswordHashing.Hash(loginDto.Password))
            {
                return Unauthorized();
            }

            var token = GenerateJwt.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpPost("auth")]
    public IActionResult Auth()
    {
        var userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        return Ok(new { UserId = userId });
    }
}