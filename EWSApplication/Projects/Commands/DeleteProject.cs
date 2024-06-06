using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using EWSDomain.Enums;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Projects.Commands;

public class DeleteProjectCommand : IRequest
{
    public required int UserId { get; set; }
    public required int ProjectId { get; set; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projects;
    private readonly ProjectService _projectService;

    public DeleteProjectCommandHandler(IProjectRepository projects, ProjectService projectService)
    {
        _projects = projects;
        _projectService = projectService;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project =
            await _projectService.GetProjectAndValidateUserAccess(request.ProjectId, request.UserId,
                ProjectRole.Participant);

        await _projects.DeleteAsync(project);
    }
}