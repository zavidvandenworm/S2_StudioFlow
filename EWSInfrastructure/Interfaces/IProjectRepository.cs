using EWSDomain.Entities;

namespace EWSInfrastructure.Interfaces;

public interface IProjectRepository
{
    Task AddAsync(Project project);

    Task<Project?> GetByIdAsync(int projectId);

    Task UpdateAsync(Project project);

    Task DeleteAsync(Project project);
}