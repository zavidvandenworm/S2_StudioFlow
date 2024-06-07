using ApplicationEF.Services;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Files.Commands;

public class DeleteFileCommand : IRequest
{
    public required int UserId { get; set; }
    public required string FileId { get; set; }
}

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
{
    private readonly FileService _fileService;
    private readonly IFileRepository _files;

    public DeleteFileCommandHandler(FileService fileService, IFileRepository files)
    {
        _fileService = fileService;
        _files = files;
    }

    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var files = await _fileService.GetFileVersionHistoryAndEnsureThatUserHasAccess(request.FileId, request.UserId);

        foreach (var file in files)
        {
            await _files.DeleteAsync(file);
        }
    }
}