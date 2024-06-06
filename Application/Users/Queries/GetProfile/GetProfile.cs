using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetProfile;

public class GetProfileQuery : IRequest<Profile>
{
    public int UserId { get; set; }
}

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Profile>
{
    private readonly IUserRepository _userRepository;
    public GetProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Profile> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _userRepository.GetUserProfile(request.UserId);
        return profile;
    }
}