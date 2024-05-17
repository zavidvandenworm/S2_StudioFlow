using System.Diagnostics;
using System.Security.Claims;
using Application.Users.Queries.GetUser;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISender _sender;
    public MainViewModel MainViewModel { get; set; }

    public HomeController(ILogger<HomeController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
        
        MainViewModel = new MainViewModel
        {
            Username = User?.Identity is not null ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value : "Guest",
            ProfilePicture = "https://images.ctfassets.net/h6goo9gw1hh6/2sNZtFAWOdP1lmQ33VwRN3/24e953b920a9cd0ff2e1d587742a2472/1-intro-photo-final.jpg?w=1200&h=992&fl=progressive&q=70&fm=jpg"
        };

        ViewBag.MainViewModel = MainViewModel;
    }

    public IActionResult Index()
    {
        var model = new HomeModel()
        {
            Authenticated = User.Identity is not null
        };
        
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}