using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;

namespace InfrastructureDapper.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<ProjectTask>> GetProjectTasks(int projectId);
        Task<ProjectTask> GetTask(int id);
        Task CreateTask(CreateTaskDto createTaskDto);
        Task AddTaskMember(int taskId, int userId);
        Task DeleteTask(int taskId);
    }
}