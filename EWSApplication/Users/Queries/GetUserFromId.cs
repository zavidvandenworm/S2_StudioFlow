using ApplicationEF.Exceptions;
using EWSDomain.Entities;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Users.Queries;

public class GetUserFromIdQuery : IRequest<User>
{
    public required int UserId { get; set; }
}

public class GetUserFromIdQueryHandler : IRequestHandler<GetUserFromIdQuery, User>
{
    private readonly IUserRepository _users;

    public GetUserFromIdQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<User> Handle(GetUserFromIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}