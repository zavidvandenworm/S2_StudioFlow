using System.Security.Claims;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUser;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Helpers;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly ISender _sender;

    public AccountController(ISender sender)
    {
        _sender = sender;
        var mainViewModel = new MainViewModel
        {
            Username = User?.Identity is not null ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value : "Guest",
            ProfilePicture = "https://images.ctfassets.net/h6goo9gw1hh6/2sNZtFAWOdP1lmQ33VwRN3/24e953b920a9cd0ff2e1d587742a2472/1-intro-photo-final.jpg?w=1200&h=992&fl=progressive&q=70&fm=jpg"
        };

        ViewBag.MainViewModel = mainViewModel;
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        
        return View();
    }
    
    [HttpGet("Login")]
    public IActionResult Login()
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        return View();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserDto model)
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        ViewBag.Errors = new List<string>();
        
        if (!ModelState.IsValid)
        {
            return View();
        }

        User? user;

        try
        {
            user = await _sender.Send(new GetUserByUsernameQuery()
            {
                Username = model.Username
            });
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.StackTrace);
            ViewBag.Errors.Add(exception.Message);
            return View();
        }

        if (user is null)
        {
            ViewBag.Errors.Add("Could not retrieve user.");
            return View();
        }

        if (!PasswordHasher.Match(model.Password, user.PasswordHash))
        {
            ViewBag.Errors.Add("Password does not match.");
            return View();
        }

        var claimsIdentity = new ClaimsIdentity(
            user.GenerateClaims(), CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddHours(4),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "App");
    }

    [Authorize]
    [HttpGet("Details")]
    public async Task<IActionResult> ManageAccount(ManageAccountModel model)
    {
        model.User = await _sender.Send(new GetUserByIdQuery()
        {
            UserId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value)
        });
        return View();
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateUserDto model)
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        ViewBag.Errors = new List<string>();
        
        if (!ModelState.IsValid)
        {
            return View();
        }

        User? user;

        try
        {
            user = await _sender.Send(new GetUserByUsernameQuery()
            {
                Username = model.Username
            });
        }
        catch (Exception exception)
        {
            ViewBag.Errors.Add(exception.Message);
            return View();
        }

        if (user is not null)
        {
            ViewBag.Errors.Add("User already exists.");
            return View();
        }

        try
        {
            user = await _sender.Send(new CreateUserCommand()
            {
                CreateUserDto = model
            });
        }
        catch (Exception exception)
        {
            ViewBag.Errors.Add(exception.Message);
            return View();
        }

        var claimsIdentity = new ClaimsIdentity(
            user.GenerateClaims(), CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddHours(4),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("LoginSuccess", new { LoggedInUsername = model.Username });
    }
}