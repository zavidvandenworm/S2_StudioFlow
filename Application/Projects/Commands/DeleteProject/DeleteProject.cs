using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand : IRequest
{
    public required int ProjectId { get; init; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projects;

    public DeleteProjectCommandHandler(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        await _projects.Delete(request.ProjectId);
    }
}