using Application;
using Domain.Entities;
using Infrastructure.SqlCommands;
using Presentation.Mvc;

namespace Presentation.Services;

public class ProjectService
{
    private readonly ProjectCommands _projectCommands;
    private readonly AuthenticationService _authentication;
    public ProjectService(ProjectCommands projectCommands, AuthenticationService authenticationService)
    {
        _projectCommands = projectCommands;
        _authentication = authenticationService;
    }

    public async Task<List<Project>> GetProjects()
    {
        return (await _projectCommands.GetProjectsThatUserParticipatesIn(_authentication.GetAuthState().Id!)).ToList();
    }
}