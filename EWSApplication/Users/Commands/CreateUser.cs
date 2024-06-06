using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using ApplicationEF.Exceptions;
using EWSDomain.Entities;
using EWSInfrastructure;
using EWSInfrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApplicationEF.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _users;

    public CreateUserCommandHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _users.GetByUsernameAsync(request.Username) is not null)
        {
            throw new UserExistsException();
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHashing.Hash(request.Password)
        };

        var added = await _users.AddAsync(user);

        return added;
    }
}