using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Interfaces;

public interface IAuthenticationService
{
    public Task<AuthState?> GetAuthState();
    public Task<bool> UserIsAuthenticated();
    public Task Authenticate(int userId);
}