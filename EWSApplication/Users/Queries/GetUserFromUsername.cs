using ApplicationEF.Exceptions;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Users.Queries;

public class GetUserFromUsernameQuery : IRequest<User>
{
    public required string Username { get; set; }
}

public class GetUserFromUsernameQueryHandler : IRequestHandler<GetUserFromUsernameQuery, User>
{
    private readonly IUserRepository _users;

    public GetUserFromUsernameQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<User> Handle(GetUserFromUsernameQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByUsernameAsync(request.Username);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}