using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWSInfrastructure.Repositories;

public class EfFileRepository : IFileRepository
{
    private readonly StudioflowDbContext _dbContext;

    public EfFileRepository(StudioflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ProjectFile file)
    {
        await _dbContext.Files.AddAsync(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ProjectFile?> GetAsync(int fileId)
    {
        return await _dbContext.Files.OrderByDescending(file => file.Version).FirstOrDefaultAsync(file => file.Id == fileId);
    }

    public async Task<ProjectFile?> GetAsync(int fileId, int version)
    {
        return await _dbContext.Files.FirstOrDefaultAsync(file => file.Id == fileId && file.Version == version);
    }

    public async Task<IEnumerable<ProjectFile?>> GetAllVersionsAsync(int fileId)
    {
        return _dbContext.Files.Where(file => file.Id == fileId).DefaultIfEmpty();
    }

    public async Task UpdateAsync(ProjectFile file)
    {
        _dbContext.Files.Update(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectFile file)
    {
        _dbContext.Files.Remove(file);
        await _dbContext.SaveChangesAsync();
    }
}