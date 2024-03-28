using Infrastructure.Factories;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        Console.WriteLine("USING INFRASTRUCTURE...");
        return services;
    }
    
}