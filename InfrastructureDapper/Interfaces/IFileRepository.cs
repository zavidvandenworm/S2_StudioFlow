using Domain.DTO;
using Domain.Entities;

namespace InfrastructureDapper.Interfaces;

public interface IFileRepository
{
    Task CreateFile(CreateFileDto createFileDto);
    Task<ProjectFile> GetFile(int fileId, bool includeContents);
    Task<List<ProjectFile>> GetProjectFiles(int projectId);
    Task DeleteFile(int fileId);
}