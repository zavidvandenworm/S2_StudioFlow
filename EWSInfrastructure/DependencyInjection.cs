using EWSInfrastructure.Interfaces;
using EWSInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EWSInfrastructure;

public static class DependencyInjection
{
    public static void AddEwsInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<StudioflowDbContext>(options => options.UseInMemoryDatabase("studioflow-inmemory"));
        services.AddScoped<IFileRepository, EfFileRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IProjectRepository, EfProjectRepository>();
        services.AddScoped<ITaskRepository, EfTaskRepository>();
    }
}