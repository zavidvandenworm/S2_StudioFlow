using ApplicationEF.Dtos;
using ApplicationEF.Services;
using AutoMapper;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Files.Commands;

public class UpdateFileCommand : IRequest
{
    public required int UserId { get; set; }
    public required string FileId { get; set; }
    public required string FileName { get; set; }
    public Stream? NewFile { get; set; }
}

public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand>
{
    private readonly FileService _fileService;
    private readonly IMapper _mapper;
    private readonly IFileRepository _files;
    
    public UpdateFileCommandHandler(FileService fileService, IFileRepository files, IMapper mapper)
    {
        _fileService = fileService;
        _files = files;
        _mapper = mapper;
    }

    public async Task Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileService.GetFileAndEnsureThatUserHasAccess(request.FileId, request.UserId);
        file.FileName = request.FileName;
        file.Version++;
        if (request.NewFile is not null)
        {
            file.FileLocation = await _fileService.SaveFileLocally(request.NewFile);
        }
        await _files.AddAsync(file);
    }
}