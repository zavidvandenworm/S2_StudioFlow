using Domain.DTO;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.SqlCommands;
using MediatR;
namespace Application.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<User>
{
    public required LoginUserDto LoginUserDto { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, User>
{
    private readonly UserCommands _userCommands;
    private readonly IMediator _mediator;
    public LoginUserCommandHandler(UserCommands userCommands, IMediator mediator)
    {
        _userCommands = userCommands;
        _mediator = mediator;
    }
    
    public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userCommands.GetUser(request.LoginUserDto.Username);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var match = PasswordHasher.Match(request.LoginUserDto.Password, user.PasswordHash);

        if (!match)
        {
            throw new PasswordInvalidException();
        }

        await _mediator.Publish(new UserLoggedInEvent(user));
        
        return user;
    }
}