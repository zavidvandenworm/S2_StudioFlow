using ApplicationEF.AiCompletions.Commands;
using ApplicationEF.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWSWebApi.Controllers;

[Authorize]
[ApiController]
[Route("ai")]
public class AiController : ControllerBase
{
    private readonly ISender _sender;

    public AiController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("completions/task")]
    public async Task<IActionResult> CompleteTask([FromBody]AiCompleteDto aiCompleteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _sender.Send(new AiCompleteTaskCommand
        {
            Title = aiCompleteDto.Title,
            Description = aiCompleteDto.Description,
            Tags = aiCompleteDto.Tags
        });

        return Ok(response);
    }
    
    [HttpPost("completions/project")]
    public async Task<IActionResult> CompleteProject([FromBody]AiCompleteDto aiCompleteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _sender.Send(new AiCompleteProjectCommand
        {
            Title = aiCompleteDto.Title,
            Description = aiCompleteDto.Description,
            Tags = aiCompleteDto.Tags
        });

        return Ok(response);
    }
}