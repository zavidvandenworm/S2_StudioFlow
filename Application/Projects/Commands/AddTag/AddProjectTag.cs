using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Projects.Commands.AddTag;

public class AddProjectTagCommand : IRequest
{
    public int ProjectId { get; set; }
    public string TagToAdd { get; set; } = null!;
}

public class AddProjectTagCommandHandler : IRequestHandler<AddProjectTagCommand>
{
    private readonly IProjectRepository _projectRepository;

    public AddProjectTagCommandHandler(ISender sender, IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task Handle(AddProjectTagCommand request, CancellationToken cancellationToken)
    {
        await _projectRepository.AddTagToProject(request.ProjectId, request.TagToAdd);
    }
}