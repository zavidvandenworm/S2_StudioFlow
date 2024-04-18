using Domain.Entities;
using Infrastructure.SqlCommands;
using MediatR;

namespace Application.Projects.Queries.GetProjectsThatUserParticipatesIn;

public class GetProjectsThatUserParticipatesInQuery : IRequest<IEnumerable<Project>>
{
    public required int UserId { get; set; }
}

public class GetProjectsThatUserParticipatesIn : IRequestHandler<GetProjectsThatUserParticipatesInQuery, IEnumerable<Project>>
{
    private readonly ProjectCommands _projectCommands;
    public GetProjectsThatUserParticipatesIn(ProjectCommands projectCommands)
    {
        _projectCommands = projectCommands;
    }
    
    public async Task<IEnumerable<Project>> Handle(GetProjectsThatUserParticipatesInQuery request, CancellationToken cancellationToken)
    {
        return await _projectCommands.GetProjectsThatUserParticipatesIn(request.UserId);
    }
}