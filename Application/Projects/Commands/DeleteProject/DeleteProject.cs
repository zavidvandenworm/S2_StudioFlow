using Domain.Events;
using Infrastructure.SqlCommands;
using MediatR;

namespace Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand : IRequest
{
    public required int ProjectId { get; init; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly ProjectCommands _projectCommands;

    public DeleteProjectCommandHandler(ProjectCommands projectCommands)
    {
        _projectCommands = projectCommands;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _projectCommands.GetProject(request.ProjectId);

        await _projectCommands.DeleteProject(entity);
        
        entity.AddDomainEvent(new ProjectDeletedEvent(entity));
    }
}