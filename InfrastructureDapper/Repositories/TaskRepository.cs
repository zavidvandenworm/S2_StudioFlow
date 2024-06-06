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

        var task = await _dbConnection.QuerySingleAsync<ProjectTask>(sql, parameters);
        task.Members = await GetTaskMembers(id);

        return task;
    }

    public async Task<IEnumerable<ProjectMember>> GetTaskMembers(int taskId)
    {
        const string sql = @"SELECT * FROM taskmembers WHERE taskId = @taskid";

        var parameters = new DynamicParameters();
        parameters.Add("@taskid", taskId);

        var members = await _dbConnection.QueryAsync<ProjectMember>(sql, parameters);

        return members;
    }

    public async Task CreateTask(CreateTaskDto createTaskDto)
    {
        const string sql = @"INSERT INTO tasks (id, projectId, name, description, deadline) VALUES (null, @projectid, @name, @description, @deadline); SELECT LAST_INSERT_ID()";

        var parameters = new DynamicParameters();
        parameters.Add("@projectid", createTaskDto.ProjectId);
        parameters.Add("@name", createTaskDto.Name);
        parameters.Add("@description", createTaskDto.Description);
        parameters.Add("@deadline", createTaskDto.Deadline);

        var taskId = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);

        foreach (var member in createTaskDto.Members)
        {
            await AddTaskMember(member.UserId, member.ProjectId);
        }

        foreach (var tag in createTaskDto.Tags)
        {
            await AddTaskTag(taskId, tag);
        }
    }

    public async Task AddTaskTag(int taskId, string tagName)
    {
        const string sql = @"INSERT INTO tasktags (id, taskId, name) VALUES (null, @taskid, @name)";

        var parameters = new DynamicParameters();
        parameters.Add("@taskid", taskId);
        parameters.Add("@name", tagName);

        await _dbConnection.ExecuteAsync(sql, parameters);
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