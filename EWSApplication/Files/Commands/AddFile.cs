using ApplicationEF.Services;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Files.Commands;

public class AddFileCommand : IRequest
{
    public required string FileName { get; set; }
    public required int ProjectId { get; set; }
    public required int UserId { get; set; }
    public required Stream FileStream { get; set; }
}

public class AddFileCommandHandler : IRequestHandler<AddFileCommand>
{
    private readonly FileService _fileService;
    private readonly IFileRepository _files;
    private readonly ProjectService _projectService;

    public AddFileCommandHandler(FileService fileService, ProjectService projectService, IFileRepository files)
    {
        _fileService = fileService;
        _projectService = projectService;
        _files = files;
    }

    public async Task Handle(AddFileCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetProjectAndValidateUserAccess(request.ProjectId, request.UserId);
        var localFile = await _fileService.SaveFileLocally(request.FileStream);
        var file = new ProjectFile
        {
            FileId = Guid.NewGuid().ToString(),
            FileLocation = localFile,
            FileName = request.FileName,
            Created = DateTimeOffset.Now,
            ProjectId = request.ProjectId
        };

        await _files.AddAsync(file);
    }
}