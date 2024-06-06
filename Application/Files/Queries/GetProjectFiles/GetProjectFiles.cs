using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Files.Queries.GetProjectFiles;

public class GetProjectFilesQuery : IRequest<List<ProjectFile>>
{
    public int ProjectId { get; set; }
    public bool IncludeContents { get; set; }
}

public class GetProjectFilesQueryHandler : IRequestHandler<GetProjectFilesQuery, List<ProjectFile>>
{
    private readonly IFileRepository _fileRepository;

    public GetProjectFilesQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<List<ProjectFile>> Handle(GetProjectFilesQuery request, CancellationToken cancellationToken)
    {
        return await _fileRepository.GetProjectFiles(request.ProjectId);
    }
}