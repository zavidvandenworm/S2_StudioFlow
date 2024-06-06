using Domain.DTO;
using Domain.Entities;

namespace InfrastructureDapper.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetById(int userId);
    public Task<User?> GetByUsername(string username);
    public Task<int?> Create(CreateUserDto createUserDto);
    public Task<Profile> GetUserProfile(int userId);
    public Task UpdateUserProfile(int userId, Profile profile);
}