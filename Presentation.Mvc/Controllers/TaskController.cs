using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Queries.GetTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

[Authorize]
[Route("tasks")]
public class TaskController : Controller
{
    private readonly ISender _sender;

    public TaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{taskid:int}")]
    public async Task<IActionResult> ViewTask(int taskid)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var model = await _sender.Send(new GetTaskQuery() { ProjectId = taskid });

        return View(model);
    }
    
    [HttpGet("create/{projectid:int}")]
    public IActionResult CreateTask(int projectid)
    {
        ViewBag.Errors = new List<string>();
        ViewBag.ProjectId = projectid;
        var model = new CreateTaskViewModel
        {
            ProjectId = projectid
        };
        return View();
    }
    
    [HttpPost("create/{projectid:int}")]
    public async Task<IActionResult> CreateTaskPost(int projectid, CreateTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors.Add("Invalid request.");
            return RedirectToAction("CreateTask", new {projectid});
        }

        model.CreateTaskDto.ProjectId = model.ProjectId;
        model.CreateTaskDto.Tags = model.TagsSeperated.Split("|").ToList();
        await _sender.Send(new CreateTaskCommand
        {
            CreateTaskDto = model.CreateTaskDto
        });
        return RedirectToAction("ViewProject", "Project", new {id = model.ProjectId});
    }
}