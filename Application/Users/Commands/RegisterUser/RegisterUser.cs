using Domain.DTO;
using Domain.Entities;
using Infrastructure.SqlCommands;
using MediatR;
using MySqlConnector;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<User>
{
    public required CreateUserDto CreateUserDto { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly UserCommands _userCommands;

    public RegisterUserCommandHandler(UserCommands userCommands)
    {
        _userCommands = userCommands;
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userCommands.CreateUser(request.CreateUserDto);
        return user;
    }
}