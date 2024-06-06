using ApplicationEF.Dtos;
using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using ApplicationEF.Users.Commands;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Projects.Commands;

public class CreateProjectCommand : IRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<ProjectMember> ProjectMembers { get; set; }
    public List<string> Tags { get; set; } = [];
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand>
{
    private readonly IProjectRepository _projects;
    private readonly IUserRepository _users;
    private readonly ProjectService _projectService;

    public CreateProjectCommandHandler(IProjectRepository projects, IUserRepository users, ProjectService projectService)
    {
        _projects = projects;
        _users = users;
        _projectService = projectService;
    }

    public async Task Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await _projectService.EnsureThatMembersExist(request.ProjectMembers);
        
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            Tags = request.Tags.Distinct().ToList(),
            ProjectMembers = request.ProjectMembers.DistinctBy(m => m.UserId).ToList(),
            Created = DateTime.Now
        };

        await _projects.AddAsync(project);
    }
}