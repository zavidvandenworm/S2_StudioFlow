using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Tasks.Queries.GetTask;

public class GetTaskQuery : IRequest<ProjectTask>
{
    public int ProjectId { get; set; }
}

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, ProjectTask>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<ProjectTask> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTask(request.ProjectId);
    }
}