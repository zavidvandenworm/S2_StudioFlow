using System.Data;
using Dapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Enums;
using InfrastructureDapper.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace InfrastructureDapper.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IDbConnection _dbConnection;

    public ProjectRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    private async Task AddProjectMember(int userId, int projectId, UserType userType)
    {
        const string sql =
            @"INSERT INTO projectmembers (`userId`, `projectId`, `role`) VALUES (@userid, @projectid, @role)";

        var parameters = new DynamicParameters();
        parameters.Add("@userid", userId);
        parameters.Add("@projectid", projectId);
        parameters.Add("@role", (int)userType);
        
        await _dbConnection.ExecuteAsync(sql, parameters);
    }

    public async Task<int> Create(CreateProjectDto createProjectDto)
    {
        const string sql =
            @"INSERT INTO `projects` (`id`, `name`, `description`, `daw`) VALUES (NULL, @name, @description, @daw); SELECT LAST_INSERT_ID()";

        var parameters = new DynamicParameters();
        parameters.Add("@name", createProjectDto.Name);
        parameters.Add("@description", createProjectDto.Description);
        parameters.Add("@daw", createProjectDto.DigitalAudioWorkstation);
        
        var projectId = await _dbConnection.ExecuteScalarAsync(sql, parameters);
        int projId = Convert.ToInt32(projectId);

        await AddProjectMember(createProjectDto.UserId, projId, UserType.User);
        await AddTagToProject(projId, Enum.GetName(createProjectDto.DigitalAudioWorkstation)!);
        
        return Convert.ToInt32(projectId);
    }

    public async Task Delete(int id)
    {
        const string sql = @"DELETE FROM projects WHERE id = @id";

        var parameters = new DynamicParameters();
        parameters.Add("@id", id);
        
        await _dbConnection.ExecuteAsync(sql, parameters);
    }

    public async Task<Project?> GetById(int id)
    {
        const string sql = @"SELECT * FROM projects WHERE id = @id";

        var parameters = new DynamicParameters();
        parameters.Add("@id", id);
        
        var projects = (await _dbConnection.QueryAsync<Project>(sql, parameters)).ToList();

        if (projects.FirstOrDefault() is null)
        {
            return null;
        }

        var project = projects.First();
        project.Tags = await GetProjectTags(id);

        return project;
    }

    public async Task<List<Project>> GetAllOfUser(int userId)
    {
        const string sql =
            @"SELECT * FROM projects WHERE id IN (SELECT projectId from projectmembers WHERE userId = @userid)";

        var parameters = new DynamicParameters();
        parameters.Add("@userid", userId);

        var projects = (await _dbConnection.QueryAsync<Project>(sql, parameters)).ToList();

        foreach (var project in projects)
        {
            project.ProjectMembers = await GetProjectMembers(project.Id);
            project.Tags = await GetProjectTags(project.Id);
        }

        return projects;
    }

    private async Task<List<Tag>> GetProjectTags(int projectId)
    {
        const string sql = @"SELECT * FROM projecttags WHERE projectId = @projectid";

        var parameters = new DynamicParameters();
        parameters.Add("@projectid", projectId);

        var tags = await _dbConnection.QueryAsync<Tag>(sql, parameters);

        return tags.ToList();
    }

    private async Task<List<ProjectMember>> GetProjectMembers(int projectId)
    {
        const string sql = @"SELECT * FROM projectmembers WHERE projectId = @projectid";

        var parameters = new DynamicParameters();
        parameters.Add("@projectid", projectId);

        var members = (await _dbConnection.QueryAsync<ProjectMember>(sql, parameters)).ToList();

        return members;
    }

    private async Task AddTagToProject(int projectId, string name)
    {
        const string sql = @"INSERT INTO projecttags (`projectId`, `name`) VALUES (@projectid, @name)";

        var parameters = new DynamicParameters();
        parameters.Add("@projectid", projectId);
        parameters.Add("@name", name);

        await _dbConnection.ExecuteAsync(sql, parameters);
    }
}