using System.Security.Claims;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.ResetPassword;
using Application.Users.Queries.GetUser;
using Domain.DTO;
using Domain.Entities;
using InfrastructureDapper.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Helpers;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly ISender _sender;

    public AccountController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        
        return View();
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "App");
        }
        return View();
    }

    [HttpPost("login")]
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
    [HttpGet("details")]
    public async Task<IActionResult> ManageAccount(ManageAccountModel model)
    {
        model.User = await _sender.Send(new GetUserByIdQuery()
        {
            UserId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value)
        });
        return View();
    }
    
    [HttpPost("register")]
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

        return RedirectToAction("Index", "App", new { LoggedInUsername = model.Username });
    }

    [HttpGet("resetpassword")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("resetpassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors.Add("Model Invalid");
            return View();
        }

        await _sender.Send(new ResetPasswordEmailSendCommand { Username = model.Username });
        return RedirectToAction("ResetConfirmation");
    }

    [HttpGet("resetconfirmation")]
    public IActionResult ResetConfirmation()
    {
        return View();
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}