using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Tasks.Queries;

public class GetTaskQuery : IRequest<ProjectTask>
{
    public required int TaskId { get; set; }
    public required int UserId { get; set; }
}

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, ProjectTask>
{
    private readonly ITaskRepository _tasks;
    private readonly TaskService _taskService;

    public GetTaskQueryHandler(ITaskRepository tasks, TaskService taskService)
    {
        _tasks = tasks;
        _taskService = taskService;
    }

    public async Task<ProjectTask> Handle(GetTaskQuery query, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetTaskAndValidateUserAccess(query.TaskId, query.UserId);

        return task;
    }
}