using FluentValidation;
using FluentValidation.AspNetCore;
using LocalLens.WebApi.Services.Auth;

namespace LocalLens.WebApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services)
    {
        services.AddContracts();
        services.AddServices();
        return services;
    }

    public static IServiceCollection AddContracts(
        this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(IAssemblyMarker).Assembly);
        return services;
    }

    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
