using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest
{
    public int TaskId { get; set; }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskRepository.DeleteTask(request.TaskId);
    }
}