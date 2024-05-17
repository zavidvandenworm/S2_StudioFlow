using System.Data;
using Dapper;
using Domain.DTO;
using Domain.Entities;
using InfrastructureDapper.Interfaces;

namespace InfrastructureDapper.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IDbConnection _dbConnection;

    public TaskRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<ProjectTask>> GetProjectTasks(int projectId)
    {
        const string sql = @"SELECT * FROM tasks WHERE projectId = @projectid";

        var parameters = new DynamicParameters();
        parameters.Add("projectid", projectId);

        var result = await _dbConnection.QueryAsync<ProjectTask>(sql, parameters);
        foreach (var projectTask in result)
        {
            Console.Write(projectTask.Name);
        }

        return result.ToList();
    }

    public async Task<ProjectTask> GetTask(int id)
    {
        const string sql = @"SELECT * FROM tasks WHERE id = @id";

        var parameters = new DynamicParameters();
        parameters.Add("@id", id);

        var task = await _dbConnection.QueryAsync<ProjectTask>(sql, parameters);

        return task.First();
    }

    public async Task CreateTask(CreateTaskDto createTaskDto)
    {
        const string sql = @"INSERT INTO tasks (id, projectId, name, description, deadline) VALUES (null, @projectid, @name, @description, @deadline)";

        var parameters = new DynamicParameters();
        parameters.Add("@projectid", createTaskDto.ProjectId);
        parameters.Add("@name", createTaskDto.Name);
        parameters.Add("@description", createTaskDto.Description);
        parameters.Add("@deadline", createTaskDto.Deadline);

        await _dbConnection.ExecuteAsync(sql, parameters);

        foreach (var member in createTaskDto.Members)
        {
            await AddTaskMember(member.UserId, member.ProjectId);
        }
    }
    
    public async Task AddTaskMember(int taskId, int userId)
    {
        const string sql = @"INSERT INTO taskmembers (id, taskId, userId) VALUES (null, @taskid, @userid)";

        var parameters = new DynamicParameters();
        parameters.Add("@taskid", taskId);
        parameters.Add("@userid", userId);

        await _dbConnection.ExecuteAsync(sql, parameters);
    }

    public async Task DeleteTask(int taskId)
    {
        const string sql = @"DELETE FROM tasks WHERE id = @id";
        
        var parameters = new DynamicParameters();
        parameters.Add("@id", taskId);

        await _dbConnection.ExecuteAsync(sql, parameters);
    }
}