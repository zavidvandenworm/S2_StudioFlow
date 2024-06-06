using System.Reflection;
using FluentEmail.Core;
using FluentEmail.SendGrid;

namespace Application;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
        Console.Out.WriteLine("SENDGRID API:" + Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
        services.AddFluentEmail("studioflow.mail@gmail.com")
            .AddSendGridSender(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
        Email.DefaultSender = new SendGridSender(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
        return services;
    }
}