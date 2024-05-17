using System.Security.Claims;
using Application.Files.Commands.CreateFile;
using Application.Files.Queries.GetProjectFiles;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Queries.GetProject;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Queries.GetProjectTasks;
using Application.Tasks.Queries.GetTask;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers;

[Authorize]
[Route("project")]
public class ProjectController : Controller
{
    private readonly ISender _sender;

    public ProjectController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ViewProject(int id)
    {
        var project = await _sender.Send(new GetProjectQuery()
        {
            ProjectId = id
        });

        if (project is null)
        {
            return RedirectToAction("Index");
        }

        project.Tasks = await _sender.Send(new GetProjectTasksQuery() { ProjectId = id });
        project.Files = await _sender.Send(new GetProjectFilesQuery { ProjectId = id });
        
        return View(project);
    }
    
    [HttpGet("create")]
    public IActionResult CreateProject()
    {
        return View();
    }

    [HttpPost("create")]
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
            var createProject = new CreateProjectDto()
            {
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                DigitalAudioWorkstation = model.DigitalAudioWorkstation,
                Tags = [Enum.GetName(model.DigitalAudioWorkstation)!]
            };
            project = await _sender.Send(new CreateProjectCommand(createProject));
        }
        catch (Exception exception)
        {
            ViewBag.Errors.Add(exception.Message);
            return View();
        }

        return RedirectToAction("ViewProject", new {id = project.Id});
    }
    
    [HttpGet("{id:int}/tasks/create")]
    public IActionResult CreateTask(int id)
    {
        return View();
    }
    
    [HttpPost("{id:int}/tasks/create")]
    public async Task<IActionResult> CreateTask(int id, CreateTaskDto model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        model.ProjectId = id;
        await _sender.Send(new CreateTaskCommand
        {
            CreateTaskDto = model
        });
        return RedirectToAction("ViewProject", new {id = id});
    }

    [HttpGet("{id:int}/files/add")]
    public IActionResult AddFile(int id)
    {
        return View();
    }

    [HttpPost("{id:int}/files/add")]
    public async Task<IActionResult> AddFile(CreateFileDto model, IFormFile file, int id)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            model.Content = memoryStream.ToArray();
        }

        model.ProjectId = id;

        await _sender.Send(new CreateFileCommand { CreateFileDto = model });
        return RedirectToAction("ViewProject", new { id = id });
    }
}