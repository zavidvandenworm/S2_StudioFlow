using ApplicationEF.Dtos;
using ApplicationEF.Tasks.Commands;
using ApplicationEF.Tasks.Queries;
using AutoMapper;
using EWSDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWSWebApi.Controllers;


[ApiController]
[Route("task")]
public class TaskController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public TaskController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateProjectTaskDto createProjectTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = HttpContext.GetUserId()!.Value;

        await _sender.Send(new CreateTaskCommand
        {
            Name = createProjectTaskDto.Name,
            Deadline = createProjectTaskDto.Deadline,
            Description = createProjectTaskDto.Description,
            Members = _mapper.Map<List<ProjectMember>>(createProjectTaskDto.Members),
            ProjectId = createProjectTaskDto.ProjectId,
            UserId = userId,
            Files = createProjectTaskDto.Files.ToList()
        });

        return Created();
    }

    [Authorize]
    [HttpGet("view")]
    public async Task<IActionResult> View([FromQuery] ViewTaskDto viewTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = HttpContext.GetUserId()!.Value;

        var task = await _sender.Send(new GetTaskQuery
        {
            TaskId = viewTaskDto.TaskId,
            UserId = userId
        });

        return Ok(_mapper.Map<ProjectTaskDto>(task));
    }

    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromBody]UpdateProjectTaskDto updateProjectTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = HttpContext.GetUserId()!.Value;

        await _sender.Send(new UpdateTaskCommand
        {
            UserId = userId,
            UpdateProjectTaskDto = updateProjectTaskDto
        });

        return Ok();
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery]DeleteTaskDto deleteTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId()!.Value;

        await _sender.Send(new DeleteTaskCommand
        {
            TaskId = deleteTaskDto.TaskId,
            UserId = userId
        });

        return Ok();
    }
}