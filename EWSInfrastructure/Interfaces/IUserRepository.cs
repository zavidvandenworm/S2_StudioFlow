using EWSDomain.Entities;

namespace EWSInfrastructure.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByUsernameAsync(string username);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}