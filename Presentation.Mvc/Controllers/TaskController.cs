using Application.Tasks.Queries.GetTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers;

[Authorize]
[Route("task")]
public class TaskController : Controller
{
    private readonly ISender _sender;

    public TaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ViewTask(int id)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var model = await _sender.Send(new GetTaskQuery() { ProjectId = id });

        return View(model);
    }
}