using ApplicationEF.Exceptions;
using AutoMapper;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;

namespace ApplicationEF.Services;

public class TaskService
{
    private readonly ProjectService _projectService;
    private readonly IProjectRepository _projects;
    private readonly ITaskRepository _tasks;
    private readonly IMapper _mapper;
    private readonly IUserRepository _users;

    public TaskService(IProjectRepository projects, IMapper mapper, IUserRepository users, ITaskRepository tasks, ProjectService projectService)
    {
        _projects = projects;
        _mapper = mapper;
        _users = users;
        _tasks = tasks;
        _projectService = projectService;
    }

    public async Task EnsureThatMembersParticipateInProject(int projectId, List<ProjectMember> members)
    {
        var project = await _projectService.GetProject(projectId);
        
        foreach (var member in members)
        {
            if (!project.ProjectMembers.Any(p => p.UserId == member.UserId))
            {
                throw new NotAllAssignedTaskMembersHaveAccessException();
            }

        }
    }

    public async Task<ProjectTask> GetTask(int taskId)
    {
        ProjectTask? task;
        try
        {
            task = await _tasks.GetAsync(taskId);
        }
        catch (Exception)
        {
            throw new GeneralException();
        }
        if (task is null)
        {
            throw new TaskNotFoundException();
        }

        return task;
    }

    public async Task<ProjectTask> GetTaskAndValidateUserAccess(int taskId, int userId)
    {
        var task = await GetTask(taskId);
        if (task is null)
        {
            throw new TaskNotFoundException();
        }
        if (task.Project.ProjectMembers.All(m => m.UserId != userId))
        {
            throw new TaskAccessDeniedException();
        }

        return task;
    }
}