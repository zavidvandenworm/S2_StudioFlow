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

    public FileService(IProjectRepository projects, IMapper mapper, IUserRepository users, IFileRepository files)
    {
        _projects = projects;
        _mapper = mapper;
        _users = users;
        _files = files;
    }

    public async Task<List<ProjectFile>> GetAndEnsureThatProjectHasFiles(int projectId, IEnumerable<int> fileIds)
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
}