using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Projects.Queries.GetProject;

public class GetProjectQuery : IRequest<Project>
{
    public int ProjectId { get; set; }
}

public class GetProjectHandler : IRequestHandler<GetProjectQuery, Project>
{
    private readonly IProjectRepository _projects;

    public GetProjectHandler(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<Project> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        return (await _projects.GetById(request.ProjectId))!;
    }
}