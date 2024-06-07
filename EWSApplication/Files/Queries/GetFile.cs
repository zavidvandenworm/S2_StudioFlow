using ApplicationEF.Services;
using EWSDomain.Entities;
using MediatR;

namespace ApplicationEF.Files.Queries;

public class GetFileCommand : IRequest<ProjectFile>
{
    public required string FileId { get; set; }
    public required int UserId { get; set; }
}

public class GetFileCommandHandler : IRequestHandler<GetFileCommand, ProjectFile>
{
    private readonly FileService _fileService;

    public GetFileCommandHandler(FileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<ProjectFile> Handle(GetFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileService.GetFileAndEnsureThatUserHasAccess(request.FileId, request.UserId);
        return file;
    }
}