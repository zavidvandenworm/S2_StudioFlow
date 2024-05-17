using Domain.DTO;
using MediatR;
using Domain.Entities;
using InfrastructureDapper.Interfaces;

namespace Application.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<Project>
{
    public CreateProjectCommand(CreateProjectDto createProjectDto)
    {
        this.CreateProjectDto = createProjectDto;
    }

    public CreateProjectDto CreateProjectDto { get; set; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly IProjectRepository _projects;

    public CreateProjectCommandHandler(IProjectRepository projects)
    {
        _projects = projects;
    }
    
    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectId = await _projects.Create(request.CreateProjectDto);
        return await _projects.GetById(projectId);
    }
}