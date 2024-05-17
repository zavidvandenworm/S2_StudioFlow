using Domain.DTO;
using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    public CreateUserDto CreateUserDto { get; set; } = null!;
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
        var userId = await _users.Create(request.CreateUserDto);
        return await _users.GetById(userId ?? -1) ?? throw new Exception("User could not be registered.");
    }
}