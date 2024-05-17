using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Files.Commands.DeleteFile;

public class DeleteFileCommand : IRequest
{
    public int FileId { get; set; }
}

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
{
    private readonly IFileRepository _fileRepository;

    public DeleteFileCommandHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.DeleteFile(request.FileId);
    }
}