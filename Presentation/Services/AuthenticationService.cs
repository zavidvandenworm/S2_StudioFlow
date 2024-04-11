using Domain.DTO;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.SqlCommands;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Task = System.Threading.Tasks.Task;

namespace Presentation.Services;

public class AuthenticationService : IAuthenticationService
{
    private const string AuthStateKey = "AuthState";
    
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly UserCommands _userCommands;
    public AuthenticationService(ProtectedSessionStorage sessionStorage, UserCommands userCommands)
    {
        _sessionStorage = sessionStorage;
        _userCommands = userCommands;
    }

    public async Task<AuthState?> GetAuthState()
    {
        AuthState? authState = (await _sessionStorage.GetAsync<AuthState>(AuthStateKey)).Value;
        return authState;
    }

    public async Task<bool> UserIsAuthenticated()
    {
        AuthState? authState = await GetAuthState();
        return authState is not null;
    }

    public async Task Authenticate(int userId)
    {
        var user = await _userCommands.GetUser(userId) ?? throw new UserNotFoundException();

        var authState = new AuthState()
        {
            User = user,
        };
        
        await _sessionStorage.SetAsync(AuthStateKey, authState);
    }
}