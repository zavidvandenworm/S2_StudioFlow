using ApplicationEF.Dtos;
using ApplicationEF.Exceptions;
using AutoMapper;
using EWSDomain.Entities;
using EWSDomain.Enums;
using EWSInfrastructure.Interfaces;

namespace ApplicationEF.Services;

public class ProjectService
{
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;
    private readonly IUserRepository _users;

    public ProjectService(IProjectRepository projects, IMapper mapper, IUserRepository users)
    {
        _projects = projects;
        _mapper = mapper;
        _users = users;
    }

    public async Task EnsureThatMembersExist(List<ProjectMember> members)
    {
        foreach (var member in members)
        {
            var user = await _users.GetByIdAsync(member.UserId);
            if (user is null)
            {
                throw new ProjectMemberNonExistantException();
            }
        }
    }
    
    public async Task EnsureThatMembersExist(List<ProjectMemberDto> members)
    {
        foreach (var member in members)
        {
            var user = await _users.GetByIdAsync(member.UserId);
            if (user is null)
            {
                throw new ProjectMemberNonExistantException();
            }
        }
    }

    public async Task<Project> GetProject(int projectId)
    {
        Project? project;
        try
        {
            project = await _projects.GetByIdAsync(projectId);
        }
        catch (Exception)
        {
            throw new GeneralException();
        }
        if (project is null)
        {
            throw new ProjectNotFoundException();
        }

        return project;
    }

    public async Task<Project> GetProjectAndValidateUserAccess(int projectId, int userId, ProjectRole role = ProjectRole.Viewer)
    {
        var project = await GetProject(projectId);

        var projectMember = project.ProjectMembers.FirstOrDefault(member => member.UserId == userId);

        if (projectMember is null)
        {
            throw new ProjectAccessDeniedException();
        }

        // switch (role)
        // {
        //     case ProjectRole.Participant when projectMember.ProjectRole == ProjectRole.Viewer:
        //         throw new InsufficientPermissionsException();
        //     case ProjectRole.Owner when projectMember.ProjectRole == ProjectRole.Viewer || projectMember.ProjectRole == ProjectRole.Participant:
        //         throw new InsufficientPermissionsException();
        // }

        return project;
    }
}