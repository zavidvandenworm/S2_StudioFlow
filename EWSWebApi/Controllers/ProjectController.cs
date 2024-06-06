using ApplicationEF.Dtos;
using ApplicationEF.Projects.Commands;
using ApplicationEF.Projects.Queries;
using AutoMapper;
using EWSDomain.Entities;
using EWSDomain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWSWebApi.Controllers;

[ApiController]
[Route("project")]
public class ProjectController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ProjectController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        createProjectDto.ProjectMembers.Add(new ProjectMemberDto
        {
            Role = ProjectRole.Owner,
            UserId = HttpContext.GetUserId()!.Value
        });

        await _sender.Send(new CreateProjectCommand
        {
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            ProjectMembers = _mapper.Map<List<ProjectMember>>(createProjectDto.ProjectMembers),
            Tags = createProjectDto.Tags
        });
        return Ok();
    }

    [Authorize]
    [HttpGet("view")]
    public async Task<IActionResult> View([FromQuery] GetProjectDto getProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId();

        var project = await _sender.Send(new GetProjectQuery
        {
            ProjectId = getProjectDto.ProjectId,
            UserId = userId!.Value
        });
        return Ok(_mapper.Map<ProjectDto>(project));
    }

    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectDto project)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId()!.Value;
        
        await _sender.Send(new UpdateProjectCommand
        {
            UpdateProjectDto = project,
            UserId = userId
        });
        return Ok();
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] DeleteProjectDto deleteProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId()!.Value;

        await _sender.Send(new DeleteProjectCommand
        {
            ProjectId = deleteProjectDto.ProjectId,
            UserId = userId
        });
        return Ok();
    }
}