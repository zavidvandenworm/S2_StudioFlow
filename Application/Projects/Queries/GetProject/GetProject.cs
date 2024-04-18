using Domain.Entities;
using Infrastructure.SqlCommands;
using MediatR;

namespace Application.Projects.Queries.GetProject;

public class GetProjectQuery : IRequest<Project>
{
    public int ProjectId { get; set; }
}

public class GetProjectHandler : IRequestHandler<GetProjectQuery, Project>
{
    private readonly ProjectCommands _projectCommands;
    public GetProjectHandler(ProjectCommands projectCommands)
    {
        _projectCommands = projectCommands;
    }
    
    public async Task<Project> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        return await _projectCommands.GetProject(request.ProjectId);
    }
}