using System.Data;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTO;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.SqlCommands;

public class ProjectCommands(SqlConnectionFactory connectionFactory, GeneralCommands generalCommands) : IProjectCommands
{
    public async Task<Project> GetProject(int id)
    {
        await using var conn = await connectionFactory.CreateOpenConnection();
        
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

    public async Task AddUserToProject(User user, Project project, ProjectRole role)
    {
        await using var conn = await connectionFactory.CreateOpenConnection();

        var comm = await SqlCommandGenerator.GenerateCommand(conn, "Project/AddProjectMember", new()
        {
            { "@userid", user.Id },
            { "@projectid", project.Id },
            { "@role", (int)role }
        });
        
        await generalCommands.ExecuteAndCheckIfSuccessful(comm);
    }

    public async Task<Project> CreateProject(CreateProjectDto createProjectDto)
    {
        await using var conn = await connectionFactory.CreateOpenConnection();

        var commProject = await SqlCommandGenerator.GenerateCommand(conn, "Project/CreateProject", new()
        {
            {"@name", createProjectDto.Name},
            {"@description", createProjectDto.Description},
        });
        
        await generalCommands.ExecuteAndCheckIfSuccessful(commProject);
        
        var id = await generalCommands.GetLastId();

        var project = new Project()
        {
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            Id = id,
            ProjectMembers = [],
            Tasks = []
        };

        await AddUserToProject(createProjectDto.Creator, project, ProjectRole.Owner);
        
        return project;
    }
}