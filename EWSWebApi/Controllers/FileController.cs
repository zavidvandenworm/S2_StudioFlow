using ApplicationEF.Dtos;
using ApplicationEF.Files.Commands;
using ApplicationEF.Files.Queries;
using ApplicationEF.Services;
using EWSDomain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWSWebApi.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly ISender _sender;
    public FileController(ISender sender)
    {
        _sender = sender;
    }

    public class AddFileLocalDto
    {
        public string NewFileName { get; set; } = null!;
        public int ProjectId { get; set; }
        public IFormFile File { get; set; } = null!;
    }

    public class UpdateFileLocalDto
    {
        public string FileId { get; set; } = null!;
        public string UpdatedFileName { get; set; } = null!;
        public IFormFile? File { get; set; }
    }
    
    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromForm]AddFileLocalDto addFileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = HttpContext.GetUserId()!.Value;
        await _sender.Send(new AddFileCommand
        {
            FileName = addFileDto.NewFileName,
            ProjectId = addFileDto.ProjectId,
            UserId = userId,
            FileStream = addFileDto.File.OpenReadStream()
        });

        return Ok();
    }

    [Authorize]
    [HttpGet("view")]
    public async Task<IActionResult> View([FromQuery]ViewProjectFileDto viewProjectFileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId()!.Value;
        
        var file = await _sender.Send(new GetFileCommand
        {
            FileId = viewProjectFileDto.FileId,
            UserId = userId
            
        });

        return Ok(file);
    }

    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromForm] UpdateFileLocalDto updateFileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId()!.Value;

        await _sender.Send(new UpdateFileCommand
        {
            FileId = updateFileDto.FileId,
            FileName = updateFileDto.UpdatedFileName,
            UserId = userId,
            NewFile = updateFileDto.File?.OpenReadStream() ?? null
        });

        return Ok();
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] DeleteFileDto deleteFileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetUserId()!.Value;

        await _sender.Send(new DeleteFileCommand
        {
            FileId = deleteFileDto.FileId,
            UserId = userId
        });

        return Ok();
    }
}