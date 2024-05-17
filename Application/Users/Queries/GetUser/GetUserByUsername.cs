using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUser;

public class GetUserByUsernameQuery : IRequest<User>
{
    public string Username { get; set; } = null!;
}

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, User>
{
    private readonly IUserRepository _users;

    public GetUserByUsernameQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        return await _users.GetByUsername(request.Username);
    }
}