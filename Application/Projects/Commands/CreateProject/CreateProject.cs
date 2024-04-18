using Domain.DTO;
using MediatR;
using Domain.Entities;
using Domain.Events;
using Infrastructure.DTO;
using Infrastructure.SqlCommands;

namespace Application.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<Project>
{
    public CreateProjectCommand()
    {
        
    }

    public CreateProjectCommand(CreateProjectDto createProjectDto)
    {
        UserId = createProjectDto.UserId;
        Name = createProjectDto.Name;
        Description = createProjectDto.Description;
    }
    
    public required int UserId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly ProjectCommands _projectCommands;

    public CreateProjectCommandHandler(ProjectCommands projectCommands)
    {
        _projectCommands = projectCommands;
    }
    
    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var createProject = new CreateProjectDto()
        {
            Name = request.Name,
            Description = request.Description,
            UserId = request.UserId
        };

        var project = await _projectCommands.CreateProject(createProject);
        
        project.AddDomainEvent(new ProjectCreatedEvent(project));
        
        return project;
    }
}