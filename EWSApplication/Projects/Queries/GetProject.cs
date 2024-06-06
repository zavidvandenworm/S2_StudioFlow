using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Projects.Queries;

public class GetProjectQuery : IRequest<Project>
{
    public required int ProjectId { get; set; }
    public required int UserId { get; set; }
}

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, Project>
{
    private readonly ProjectService _projectService;

    public GetProjectQueryHandler(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<Project> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetProjectAndValidateUserAccess(request.ProjectId, request.UserId);
        return project;
    }
}