using System.Security.Claims;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Queries.GetProject;
using Application.Projects.Queries.GetProjectsThatUserParticipatesIn;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Queries.GetProjectTasks;
using Application.Tasks.Queries.GetTask;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

[Authorize]
[Route("home")]
public class AppController : Controller
{
    private readonly ISender _sender;

    public AppController(ISender sender)
    {
        _sender = sender;
        
        var mainViewModel = new MainViewModel
        {
            Username = User?.Identity is not null ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value : "Guest",
            ProfilePicture = "https://images.ctfassets.net/h6goo9gw1hh6/2sNZtFAWOdP1lmQ33VwRN3/24e953b920a9cd0ff2e1d587742a2472/1-intro-photo-final.jpg?w=1200&h=992&fl=progressive&q=70&fm=jpg"
        };

        ViewBag.MainViewModel = mainViewModel;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(AppHomeModel model)
    {
        model.Username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userid = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        model.Projects = (await _sender.Send(new GetProjectsThatUserParticipatesInQuery()
        {
            UserId = userid
        })).ToList();
        
        return View(model);
    }


    
    

    public IActionResult ProjectCreateSuccess(Project model)
    {
        return View(model);
    }
}