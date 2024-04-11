using Domain.Events;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Users.EventHandlers;

public class UserLoggedInEventHandler : INotificationHandler<UserLoggedInEvent>
{
    private readonly IAuthenticationService _authenticationService;
    public UserLoggedInEventHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
    {
        await _authenticationService.Authenticate(notification.User.Id);
        Console.WriteLine($"User {notification.User.Id} logged in successfully.");
    }
}