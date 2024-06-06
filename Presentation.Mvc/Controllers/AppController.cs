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
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(AppHomeModel model)
    {
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