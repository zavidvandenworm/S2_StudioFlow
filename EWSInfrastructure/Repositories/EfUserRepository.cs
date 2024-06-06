using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWSInfrastructure.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly StudioflowDbContext _dbContext;

    public EfUserRepository(StudioflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user)
    {
        var addedUser = await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return addedUser.Entity;
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
    
}