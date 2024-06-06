using ApplicationEF.Exceptions;
using ApplicationEF.Projects.Commands;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public required User User { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _users;

    public DeleteUserCommandHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.User.Id);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _users.DeleteAsync(user);
    }
}