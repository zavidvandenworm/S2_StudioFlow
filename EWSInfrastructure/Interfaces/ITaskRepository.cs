using EWSDomain.Entities;

namespace EWSInfrastructure.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(ProjectTask task);

    Task<ProjectTask?> GetAsync(int taskId);

    Task UpdateAsync(ProjectTask task);

    Task DeleteAsync(ProjectTask task);
}