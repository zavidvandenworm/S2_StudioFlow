using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Files.Queries.GetFile;

public class GetFileQuery : IRequest<ProjectFile>
{
    public int FileId { get; set; }
}

public class GetFileQueryHandler : IRequestHandler<GetFileQuery, ProjectFile>
{
    private readonly IFileRepository _fileRepository;

    public GetFileQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<ProjectFile> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        return await _fileRepository.GetFile(request.FileId);
    }
}