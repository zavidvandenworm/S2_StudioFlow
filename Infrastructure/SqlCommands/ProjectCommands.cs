using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTO;
using Infrastructure.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.SqlCommands;

public class ProjectCommands : SqlCommandHelper
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ProjectCommands(SqlConnectionFactory connectionFactory) : base(connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<Project> GetProject(int id)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();
        
        var comm = await SqlCommandGenerator.GenerateCommand(conn, "Project/GetProjectById", new(){ { "@id", id } });

        await using var reader = await comm.ExecuteReaderAsync();

        Project? project = null;

        while (await reader.ReadAsync())
        {
            project = new Project()
            {
                Name = reader.GetString("name"),
                Description = reader.GetString("description"),
                Id = reader.GetInt16("id"),
                ProjectMembers = [],
                Tasks = []
            };
        }

        return project ?? throw new Exception("Failed to get project.");
    }

    public async Task AddUserToProject(int userId, Project project, ProjectRole role)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();

        var comm = await SqlCommandGenerator.GenerateCommand(conn, "Project/AddProjectMember", new()
        {
            { "@userid", userId },
            { "@projectid", project.Id },
            { "@role", (int)role }
        });
        
        await SqlChecks.ExecuteAndCheckIfSuccessful(comm);
    }
    
    public async Task<Project> CreateProject(CreateProjectDto createProjectDto)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();

        var commProject = await SqlCommandGenerator.GenerateCommand(conn, "Project/CreateProject", new()
        {
            {"@name", createProjectDto.Name},
            {"@description", createProjectDto.Description},
        });
        
        await SqlChecks.ExecuteAndCheckIfSuccessful(commProject);

        var id = await GetLastId(conn);

        var project = new Project()
        {
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            Id = id,
            ProjectMembers = [],
            Tasks = []
        };

        await AddUserToProject(createProjectDto.UserId, project, ProjectRole.Owner);
        
        return project;
    }

    public async Task DeleteProject(Project project)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();

        var comm = await SqlCommandGenerator.GenerateCommand(conn, "Project/DeleteProject", new()
        {
            {"@projectid", project.Id}
        });

        await SqlChecks.ExecuteAndCheckIfSuccessful(comm);
    }
}