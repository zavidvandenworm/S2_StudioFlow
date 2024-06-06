using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWSInfrastructure.Repositories;

public class EfProjectRepository : IProjectRepository
{
    private readonly StudioflowDbContext _dbContext;

    public EfProjectRepository(StudioflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Project project)
    {
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Project?> GetByIdAsync(int projectId)
    {
        return await _dbContext.Projects
            .Include(project => project.Tasks)
            .ThenInclude(task => task.Members)
            .Include(project => project.ProjectMembers)
            .Include(project => project.Files)
            .FirstOrDefaultAsync(project => project.Id == projectId);
    }

    public async Task UpdateAsync(Project project)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
    }
}