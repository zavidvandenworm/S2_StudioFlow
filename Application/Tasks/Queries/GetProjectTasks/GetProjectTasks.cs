using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Tasks.Queries.GetProjectTasks;

public class GetProjectTasksQuery : IRequest<List<ProjectTask>>
{
    public int ProjectId { get; set; }
}

public class GetProjectTasksQueryHandler : IRequestHandler<GetProjectTasksQuery, List<ProjectTask>>
{
    private readonly ITaskRepository _taskRepository;

    public GetProjectTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<ProjectTask>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetProjectTasks(request.ProjectId);
    }
}