using ApplicationEF.Dtos;
using ApplicationEF.Services;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Tasks.Commands;

public class CreateTaskCommand : IRequest
{
    public required int UserId { get; set; }
    public required int ProjectId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required List<ProjectMember> Members { get; set; }
    public required List<ProjectFileReferenceDto> Files { get; set; }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly TaskService _taskService;
    private readonly ProjectService _projectService;
    private readonly FileService _fileService;

    public CreateTaskCommandHandler(ITaskRepository tasks, TaskService taskService, ProjectService projectService, FileService fileService)
    {
        _tasks = tasks;
        _taskService = taskService;
        _projectService = projectService;
        _fileService = fileService;
    }

    public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        await _projectService.GetProjectAndValidateUserAccess(request.ProjectId, request.UserId);
        await _taskService.EnsureThatMembersParticipateInProject(request.ProjectId, request.Members);
        
        var files = await _fileService.GetFilesByIdsAndEnsureThatProjectHasFiles(request.ProjectId, request.Files.Select(f => f.FileId));
        
        var task = new ProjectTask
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            Deadline = request.Deadline,
            Members = request.Members.DistinctBy(m => m.UserId).ToList(),
            ProjectFiles = files
        };

        await _tasks.AddAsync(task);
    }
}