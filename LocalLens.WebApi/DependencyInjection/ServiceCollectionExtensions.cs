using FluentValidation;
using FluentValidation.AspNetCore;
using Google;
using LocalLens.WebApi.Constants.Configurations;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LocalLens.WebApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddContracts();
        services.AddServices();
        services.AddConfigurations(configuration);
        services.AddDatabase(configuration);
        services.AddJwtAuthenticationSettings(configuration);

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

    public static IServiceCollection AddConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
          .AddOptions<GoogleAuthConfig>()
          .Bind(configuration.GetSection(GoogleAuthConfig.Key))
          .ValidateDataAnnotations()
          .ValidateOnStart();

        services
          .AddOptions<JwtConfig>()
          .Bind(configuration.GetSection(JwtConfig.Key))
          .ValidateDataAnnotations()
          .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContext<LocalLensDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));


        services
            .AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<LocalLensDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddJwtAuthenticationSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtConfig.Key);

        var secretKeyInString = jwtSettings["SecretKey"];
        var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyInString!);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyInBytes)
            });

        return services;
    }
}
