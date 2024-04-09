using Domain.DTO;
using Domain.Entities;
using Infrastructure.SqlCommands;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Task = System.Threading.Tasks.Task;

namespace Presentation.Services;

public class AuthenticationService
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
        var authState = new AuthState()
        {
            UserId = userId
        };
        
        await _sessionStorage.SetAsync(AuthStateKey, authState);
    }

    public async Task Deauthenticate()
    {
        await _sessionStorage.DeleteAsync(AuthStateKey);
    }

    public async Task Login(LoginUserDto loginDto)
    {
        var user = await _userCommands.GetUser(loginDto.Username);
    }
}