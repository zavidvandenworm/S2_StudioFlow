using Domain.Entities;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest
{
    public int UserId { get; set; }
    public Profile Profile { get; set; } = null!;
}

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateUserProfile(request.UserId, request.Profile);
    }
}