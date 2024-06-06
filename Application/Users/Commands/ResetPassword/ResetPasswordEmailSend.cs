using FluentEmail.Core;
using InfrastructureDapper.Interfaces;
using MediatR;

namespace Application.Users.Commands.ResetPassword;

public class ResetPasswordEmailSendCommand : IRequest
{
    public string Username { get; set; } = null!;
}

public class ResetPasswordEmailSendCommandHandler : IRequestHandler<ResetPasswordEmailSendCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IFluentEmail _email;

    public ResetPasswordEmailSendCommandHandler(IUserRepository userRepository, IFluentEmail email)
    {
        _userRepository = userRepository;
        _email = email;
    }

    public async Task Handle(ResetPasswordEmailSendCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsername(request.Username);
        if (user is null)
        {
            Console.Out.WriteLine("No user found to reset");
            return;
        }

        string body = @$"
<h1>Hello, {user.Username}!</h1>
<span>It seems you have requested to reset your password.</span>
";

        var response = await _email.SetFrom("studioflow.mail").To(user.Email).Body(body, true).Subject("Password reset").Tag("tag").SendAsync();
        response.ErrorMessages.ForEach(e => Console.WriteLine(e));
    }
}