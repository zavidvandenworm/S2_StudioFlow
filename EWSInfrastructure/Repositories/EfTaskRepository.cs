using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWSInfrastructure.Repositories;

public class EfTaskRepository : ITaskRepository
{
    private readonly StudioflowDbContext _dbContext;

    public EfTaskRepository(StudioflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ProjectTask task)
    {
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ProjectTask?> GetAsync(int taskId)
    {
        return await _dbContext.Tasks
            .Include(task => task.Members)
            .Include(task => task.Project)
            .ThenInclude(project => project.ProjectMembers)
            .FirstOrDefaultAsync(task => task.Id == taskId);
    }

    public async Task UpdateAsync(ProjectTask task)
    {
        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectTask task)
    {
        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
    }
}