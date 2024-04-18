using System.Security.Claims;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Queries.GetProjectsThatUserParticipatesIn;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.DTO;
using Infrastructure.SqlCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

[Authorize]
[Route("App")]
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
        model.Username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        var userid = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        model.Projects = (await _sender.Send(new GetProjectsThatUserParticipatesInQuery()
        {
            UserId = userid
        })).ToList();
        
        return View(model);
    }

    [HttpGet("Project/Create")]
    public IActionResult CreateProject()
    {
        return View();
    }

    [HttpPost("Project/Create")]
    public async Task<IActionResult> CreateProject(CreateProjectDto model)
    {
        ViewBag.Errors = new List<string>();
        if (!ModelState.IsValid)
        {
            ViewBag.Errors.Add("Submitted form is invalid!");
            return View();
        }

        model.UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        Project project;

        try
        {
            project = await _sender.Send(new CreateProjectCommand()
            {
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
            });
        }
        catch (Exception exception)
        {
            ViewBag.Errors.Add(exception.Message);
            return View();
        }

        return RedirectToAction("ProjectCreateSuccess", project);
    }

    public IActionResult ProjectCreateSuccess(Project model)
    {
        return View(model);
    }
}