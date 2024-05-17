using Domain.DTO;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace InfrastructureDapper.Interfaces;

public interface IProjectRepository
{
    public Task<int> Create(CreateProjectDto createProjectDto);
    public Task Delete(int id);
    public Task<Project?> GetById(int id);
    public Task<List<Project>> GetAllOfUser(int userId);
}