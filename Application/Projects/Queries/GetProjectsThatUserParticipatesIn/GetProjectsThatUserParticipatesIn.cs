using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Projects.Queries.GetProjectsThatUserParticipatesIn;

public class GetProjectsThatUserParticipatesInQuery : IRequest<IEnumerable<Project>>
{
    public required int UserId { get; set; }
}

public class GetProjectsThatUserParticipatesIn : IRequestHandler<GetProjectsThatUserParticipatesInQuery, IEnumerable<Project>>
{
    private readonly IProjectRepository _projects;

    public GetProjectsThatUserParticipatesIn(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<IEnumerable<Project>> Handle(GetProjectsThatUserParticipatesInQuery request, CancellationToken cancellationToken)
    {
        return await _projects.GetAllOfUser(request.UserId);
    }
}