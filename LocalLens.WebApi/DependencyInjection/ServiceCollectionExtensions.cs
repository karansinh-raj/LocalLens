﻿using Amazon.S3;
using FluentValidation;
using FluentValidation.AspNetCore;
using LocalLens.WebApi.Constants.Configurations;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Services.Auth;
using LocalLens.WebApi.Services.PlacesTypes;
using LocalLens.WebApi.Services.Preferences;
using LocalLens.WebApi.Services.UserPreferences;
using LocalLens.WebApi.Services.Questions;
using LocalLens.WebApi.Services.UserDailyActivities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenAI_API;
using LocalLens.WebApi.Services.Places;
using LocalLens.WebApi.Services.UserPredicatedPlaces;

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
		services.AddMappings();
		services.AddHttpClients(configuration);

		services.AddSwaggerDocumentation();
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
		services.AddScoped<IPlaceTypesService, PlaceTypesService>();
		services.AddScoped<IPreferencesService, PreferenceService>();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();
		services.AddScoped<IQuestionsService, QuestionsService>();
        services.AddScoped<IUserDailyActivityService, UserDailyActivityService>();
        services.AddScoped<IPlacesService, PlacesService>();

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
			.AddIdentity<User, IdentityRole<Guid>>(options =>
			{
				options.User.RequireUniqueEmail = true;
			})
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

	public static IServiceCollection AddMappings(
		this IServiceCollection services)
	{
		services.AddAutoMapper([typeof(IAssemblyMarker).Assembly]);
		return services;
	}

    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
		IConfiguration configuration)
    {
        services.AddHttpClient<IUserPredicatedPlaces, UserPredicatedPlaces>(client =>
        {
			var baseUrl = configuration["PlacesApiBaseUrl"];
            client.BaseAddress = new Uri(baseUrl!);
        });
        return services;
    }

    //  public static WebApplicationBuilder AddOpenAIChatGPT(
    //this WebApplicationBuilder builder, 
    //IConfiguration configuration)
    //  {
    //      var chatGptKey = configuration["OpenAIChatGpt:Key"];

    //      var chat = new OpenAIAPI(chatGptKey);

    //      builder.Services.AddSingleton(chat);

    //      return builder;
    //  }
}
