using System.Reflection;
using ApplicationEF.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationEF;

public static class DependencyInjection
{
    public static void AddEwsApplication(this IServiceCollection services)
    {
        services.AddScoped<ProjectService>();
        services.AddScoped<TaskService>();
        services.AddScoped<FileService>();
        services.AddScoped<AiService>();
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
    }
}