using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUser;

public class GetUserByIdQuery : IRequest<User>
{
    public int UserId { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _users;

    public GetUserQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _users.GetById(request.UserId);
    }
}