using System.Security.Claims;
using Application.Files.Commands.CreateFile;
using Application.Files.Queries.GetProjectFiles;
using Application.Projects.Commands.AddTag;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Queries.GetProject;
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
    public async Task<IActionResult> CreateProject(CreateProjectViewModel model)
    {
        ViewBag.Errors = new List<string>();
        if (!ModelState.IsValid)
        {
            ViewBag.Errors.Add("Submitted form is invalid!");
            return View();
        }

        model.CreateProjectDto.UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

        Project project;

        List<string> tags = model.TagsSplit.Split("|").ToList();
        tags.Add(Enum.GetName(model.CreateProjectDto.DigitalAudioWorkstation)!);
        
        try
        {
            var createProject = new CreateProjectDto()
            {
                UserId = model.CreateProjectDto.UserId,
                Name = model.CreateProjectDto.Name,
                Description = model.CreateProjectDto.Description,
                DigitalAudioWorkstation = model.CreateProjectDto.DigitalAudioWorkstation,
                Tags = tags
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

    [HttpGet("{id:int}/files/add")]
    public IActionResult AddFile(int id)
    {
        ViewBag.ProjectId = id;
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

    [HttpGet("{id:int}/delete")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        Project project;
        try
        {
            project = await _sender.Send(new GetProjectQuery { ProjectId = id });
        }
        catch (Exception)
        {
            return RedirectToAction("Index", "App");
        }

        var model = new DeleteProjectViewModel();
        model.Project = project;

        return View(model);
    }
    
    [HttpPost("{id:int}/delete")]
    public async Task<IActionResult> DeleteProject(DeleteProjectViewModel model, int id)
    {
        Project project;
        try
        {
            project = await _sender.Send(new GetProjectQuery { ProjectId = id });
        }
        catch (Exception)
        {
            return NotFound();
        }

        if (model.ProjectNameVerification != project.Name)
        {
            ViewBag.Errors.Add("Project name does not match!");
            return View();
        }

        await _sender.Send(new DeleteProjectCommand { ProjectId = id });

        return RedirectToAction("ProjectDeletionSuccess");
    }

    [HttpGet("deletionsuccess")]
    public IActionResult ProjectDeletionSuccess()
    {
        return View();
    }

    [HttpGet("{id:int}/tag/add")]
    public IActionResult AddTag(int id)
    {
        ViewBag.ProjectId = id;
        return View();
    }
    
    [HttpPost("{id:int}/tag/add")]
    public async Task<IActionResult> AddTag(int id, AddTagViewModel model)
    {
        ViewBag.Errors = new List<string>();
        if (!ModelState.IsValid)
        {
            View();
        }
        
        Project project;
        try
        {
            project = await _sender.Send(new GetProjectQuery { ProjectId = id });
        }
        catch (Exception)
        {
            ViewBag.Errors.Add("Error occured.");
            return View();
        }

        if (project.Tags.Any(t => t.Name == model.NewTagName))
        {
            ViewBag.Errors.Add("Tag with name already exists.");
            return View();
        }

        await _sender.Send(new AddProjectTagCommand { ProjectId = id, TagToAdd = model.NewTagName });

        return RedirectToAction("ViewProject", new{id = id});
    }
}