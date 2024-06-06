using ApplicationEF.Dtos;
using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using AutoMapper;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Tasks.Commands;

public class UpdateTaskCommand : IRequest
{
    public required UpdateProjectTaskDto UpdateProjectTaskDto { get; set; }
    public required int UserId { get; set; }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly TaskService _taskService;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(ITaskRepository tasks, IMapper mapper, TaskService taskService)
    {
        _tasks = tasks;
        _mapper = mapper;
        _taskService = taskService;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetTaskAndValidateUserAccess(request.UpdateProjectTaskDto.TaskId, request.UserId);

        request.UpdateProjectTaskDto.Members = request.UpdateProjectTaskDto.Members.DistinctBy(m => m.UserId).ToList();
        request.UpdateProjectTaskDto.ProjectFiles = request.UpdateProjectTaskDto.ProjectFiles.DistinctBy(f => f.FileId).ToList();

        var updatedTask = _mapper.Map(request.UpdateProjectTaskDto, task);
        
        await _tasks.UpdateAsync(updatedTask);
    }
}