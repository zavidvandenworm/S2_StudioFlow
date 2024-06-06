using ApplicationEF.Exceptions;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Users.Commands;

public class UpdateUserCommand : IRequest
{
    public required User User { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _users;

    public UpdateUserCommandHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.User.Id);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _users.UpdateAsync(request.User);
    }
}