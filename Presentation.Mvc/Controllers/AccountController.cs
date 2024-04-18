using System.Security.Claims;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.Helpers;
using Infrastructure.SqlCommands;
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
    private readonly UserCommands _userCommands;

    public AccountController(UserCommands userCommands)
    {
        _userCommands = userCommands;
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [Authorize]
    [HttpGet("Login/Success")]
    public IActionResult LoginSuccess(LoginSuccessModel model)
    {
        return View(model);
    }
    
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserDto model)
    {
        ViewBag.Errors = new List<string>();
        
        if (!ModelState.IsValid)
        {
            return View();
        }

        User? user;

        try
        {
            user = await _userCommands.GetUser(model.Username);
        }
        catch (Exception exception)
        {
            ViewBag.Errors.Add(exception.Message);
            return View();
        }

        if (user is null)
        {
            ViewBag.Errors.Add("Could not retrieve user.");
            return View();
        }

        if (!PasswordHasher.Match(model.Password, user!.PasswordHash))
        {
            ViewBag.Errors.Add("Password does not match.");
            Console.WriteLine("password not matchy");
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
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateUserDto model)
    {
        ViewBag.Errors = new List<string>();
        
        if (!ModelState.IsValid)
        {
            return View();
        }

        User? user;

        try
        {
            user = await _userCommands.GetUser(model.Username);
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
            user = await _userCommands.CreateUser(model);
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