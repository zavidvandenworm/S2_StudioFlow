using MediatR;
using Domain.Entities;
using Domain.Events;

namespace Application.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = new Project()
        {
            Name = request.Name,
            Description = request.Description,
            Tasks = [],
            ProjectMembers = []
        };
        
        entity.AddDomainEvent(new ProjectCreatedEvent(entity));

        return 1;
    }
}