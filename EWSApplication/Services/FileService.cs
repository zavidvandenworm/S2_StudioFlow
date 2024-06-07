using ApplicationEF.Exceptions;
using AutoMapper;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;

namespace ApplicationEF.Services;

public class FileService
{
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;
    private readonly IUserRepository _users;
    private readonly IFileRepository _files;
    private readonly ProjectService _projectService;

    private const string FileSaveDir = "./user-files/";

    public FileService(IProjectRepository projects, IMapper mapper, IUserRepository users, IFileRepository files, ProjectService projectService)
    {
        _projects = projects;
        _mapper = mapper;
        _users = users;
        _files = files;
        _projectService = projectService;

        if (!Directory.Exists(FileSaveDir))
        {
            Directory.CreateDirectory(FileSaveDir);
        }
    }

    public async Task<List<ProjectFile>> GetFilesByIdsAndEnsureThatProjectHasFiles(int projectId, IEnumerable<int> fileIds)
    {
        var project = await _projects.GetByIdAsync(projectId);

        List<ProjectFile> files = [];

        foreach (int id in fileIds)
        {
            var file = project!.Files.FirstOrDefault(f => f.Id == id);
            if (file is null)
            {
                throw new FileNotFoundException();
            }
            files.Add(file);
        }

        return files;
    }

    public async Task<ProjectFile> GetFileAndEnsureThatUserHasAccess(string fileId, int userId)
    {
        var file = await _files.GetAsync(fileId);
        if (file is null)
        {
            throw new ProjectFileNotFoundException();
        }

        var project = await _projectService.GetProjectAndValidateUserAccess(file.ProjectId, userId);

        return file;
    }

    public async Task<List<ProjectFile>> GetFileVersionHistoryAndEnsureThatUserHasAccess(string fileId, int userId)
    {
        var file = await _files.GetAsync(fileId);
        if (file is null)
        {
            throw new ProjectFileNotFoundException();
        }
        
        var project = await _projectService.GetProjectAndValidateUserAccess(file.ProjectId, userId);
        var files = await _files.GetAllVersionsAsync(fileId);

        return files.ToList()!;
    }

    public async Task<string> SaveFileLocally(Stream fileStream)
    {
        var fileLocation = Path.Combine(FileSaveDir, Guid.NewGuid() + ".userdata");
        
        await using var fs = File.Create(fileLocation);
        await fileStream.CopyToAsync(fs);

        return fileLocation;
    }
}