using Domain.DTO;
using Domain.Entities;
using InfrastructureDapper.Interfaces;
using InfrastructureDapper.Repositories;
using MediatR;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest
{
    public CreateTaskDto CreateTaskDto { get; set; } = null!;
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskRepository.CreateTask(request.CreateTaskDto);
    }
}