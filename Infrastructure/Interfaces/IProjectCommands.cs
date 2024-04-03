using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTO;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Interfaces;

public interface IProjectCommands
{
    Task<Project> GetProject(int id);
    Task AddUserToProject(User user, Project project, ProjectRole role);
    Task<Project> CreateProject(CreateProjectDto createProjectDto);
}