using EWSDomain.Entities;

namespace EWSInfrastructure.Interfaces;

public interface IFileRepository
{
    Task AddAsync(ProjectFile file);

    Task<ProjectFile?> GetAsync(int fileId);
    Task<ProjectFile?> GetAsync(int fileId, int version);
    Task<IEnumerable<ProjectFile?>> GetAllVersionsAsync(int fileId);

    Task UpdateAsync(ProjectFile file);

    Task DeleteAsync(ProjectFile file);
}