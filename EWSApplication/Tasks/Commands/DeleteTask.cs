using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Tasks.Commands;

public class DeleteTaskCommand : IRequest
{
    public required int UserId { get; set; }
    public required int TaskId { get; set; }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly TaskService _taskService;

    public DeleteTaskCommandHandler(ITaskRepository tasks, TaskService taskService)
    {
        _tasks = tasks;
        _taskService = taskService;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetTaskAndValidateUserAccess(request.TaskId, request.UserId);

        await _tasks.DeleteAsync(task);
    }
}