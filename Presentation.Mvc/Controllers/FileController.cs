using Application.Files.Queries.GetFile;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Presentation.Mvc.Models;

namespace Presentation.Mvc.Controllers;

[Authorize]
[Route("project/{projectid:int}/file")]
public class FileController : Controller
{
    private readonly ISender _sender;

    public FileController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{fileid:int}")]
    public async Task<IActionResult> ViewFile(int projectid, int fileid)
    {
        var model = new ViewFileViewModel();
        ProjectFile file;
        try
        {
            file = await _sender.Send(new GetFileQuery { FileId = fileid });
        }
        catch (Exception)
        {
            return Content("could not find file");
        }

        model.ProjectFile = file;
        model.ProjectId = projectid;

        return View(model);
    }
    
    [HttpGet("{fileid:int}/download")]
    public async Task<IActionResult> DownloadFile(int projectid, int fileid)
    {
        ProjectFile file;
        try
        {
            file = await _sender.Send(new GetFileQuery { FileId = fileid, IncludeContents = true});
        }
        catch (Exception)
        {
            return Content("could not find file");
        }

        return File(file.FileContents, "application/octet-stream", file.FileName);
    }
}