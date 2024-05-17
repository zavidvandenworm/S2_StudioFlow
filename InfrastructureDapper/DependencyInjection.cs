using System.Data;
using System.Data.SqlClient;
using InfrastructureDapper.Interfaces;
using InfrastructureDapper.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureDapper;

public static class DependencyInjection
{
    public static void AddInfra(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
    }
}