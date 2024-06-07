using EWSDomain.Entities;

namespace EWSInfrastructure.Interfaces;

public interface IFileRepository
{
    Task AddAsync(ProjectFile file);

    Task<ProjectFile?> GetAsync(string fileId);
    Task<ProjectFile?> GetAsync(string fileId, int version);
    Task<IEnumerable<ProjectFile?>> GetAllVersionsAsync(string fileId);

    Task UpdateAsync(ProjectFile file);

    Task DeleteAsync(ProjectFile file);
}