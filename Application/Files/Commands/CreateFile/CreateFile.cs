using Domain.DTO;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Files.Commands.CreateFile;

public class CreateFileCommand : IRequest
{
    public CreateFileDto CreateFileDto { get; set; } = null!;
}

public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand>
{
    private readonly IFileRepository _fileRepository;

    public CreateFileCommandHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.CreateFile(request.CreateFileDto);
    }
}