using AutoMapper;
using Google.Apis.Auth;
using LocalLens.WebApi.Constants.Configurations;
using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Errors.Auth;
using LocalLens.WebApi.Messages.Auth;
using LocalLens.WebApi.ResultPattern;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LocalLens.WebApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly LocalLensDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly GoogleAuthConfig _googleAuthConfig;
    private readonly JwtConfig _jwtConfig;

    public AuthService(
        ILogger<AuthService> logger, 
        IOptions<GoogleAuthConfig> googleAuthConfig,
        IOptions<JwtConfig> jwtConfig,
        LocalLensDbContext dbContext,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
        _mapper = mapper;
        _googleAuthConfig = googleAuthConfig.Value;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<ResultT<GoogleLoginResponse>> GoogleLoginAsync(
        GoogleLoginRequest request,
        CancellationToken ct)
    {
        var userInfo = await 
            ValidateAccessTokenAndGetUserInfoAsync(request.Token);

        if (userInfo is null)
        {
            return AuthErrors.InvalidAccessToken;
        }

        var userId = Guid.NewGuid();

        var resultOfCreateUser = await
            CreateUserIfNotExists(userId, userInfo.Email, userInfo.FirstName, userInfo.LastName, userInfo.ProfileUrl);

        if (resultOfCreateUser.Item1 && resultOfCreateUser.Item2.IsDeleted == true)
        {
            return AuthErrors.UserAccountDeleted;
        }

        if (!resultOfCreateUser.Item1)
        {
            return AuthErrors.UserCreateFailure;
        }

        var claims = GetClaims(userInfo, resultOfCreateUser.Item2.Id);
        var (accessToken, accessTokenExpiry) = GenerateAccessToken(claims);

        var response = new GoogleLoginResponse
        {
            AccessToken = accessToken,
            ExpiryTimeUtc = accessTokenExpiry
        };

        return (response, AuthMessages.LoginSuccess);
    }

    private async Task<GoogleUserDetailsResponse?> ValidateAccessTokenAndGetUserInfoAsync(string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = _googleAuthConfig.AppIds
        };

        try
        {
            var result = await GoogleJsonWebSignature.ValidateAsync(idToken, settings).ConfigureAwait(false);
            return new GoogleUserDetailsResponse
            {
                Id = result.Subject,
                Email = result.Email,
                FirstName = result.GivenName,
                LastName = result.FamilyName,
                ProfileUrl = result.Picture
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during validating Google token");
            return null;
        }
    }

    private async Task<(bool, User)> CreateUserIfNotExists(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string profileUrl)
    {
        var userFromDb = await _userManager.FindByEmailAsync(email);
        if (userFromDb is not null)
        {
            return await Task.FromResult((true, userFromDb));
        }

        var user = new User
        {
            Id = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            NormalizedEmail = email.ToUpper(),
            UserName = email,
            NormalizedUserName = email.ToUpper(),
            ProfileUrl = profileUrl,
            LoginProvider = LoginProvider.Google,
            CreatedOnUtc = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user);
        var resultOfCreateUser = await _dbContext.SaveChangesAsync();
        return (resultOfCreateUser > 0, user);
    }

    private (string, DateTime) GenerateAccessToken(
        IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            expires: DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpirationInMinutes),
            claims: claims,
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationInMinutes);

        return (accessToken, accessTokenExpiration);
    }

   private Claim[] GetClaims(
       GoogleUserDetailsResponse user,
       Guid userId) =>
       [
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
           new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
           new Claim(JwtRegisteredClaimNames.Email, user.Email!.ToString()),
           new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName.ToString()),
       ];

    public async Task<ResultT<string>> UpdateProfileAsync(
        UpdateUserRequest request,
        Guid userId,
        CancellationToken ct)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        if (existingUser == null)
        {
            return AuthErrors.UserNotFound;
        }

        _mapper.Map(request, existingUser);
        existingUser.UpdatedOnUtc = DateTime.UtcNow;
        _dbContext.Users.Update(existingUser);
        var resultOfUpdateUser = await _dbContext.SaveChangesAsync(ct);

        if (resultOfUpdateUser > 0)
        {
            return (AuthMessages.UserUpdateSuccess, AuthMessages.UserUpdateSuccess);
        }

        return AuthErrors.UserUpdateFailure;
    }

    public async Task<ResultT<string>> DeleteProfileAsync(
        Guid userId,
        CancellationToken ct)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        if (existingUser == null)
        {
            return AuthErrors.UserNotFound;
        }

        existingUser.DeletedOnUtc = DateTime.UtcNow;
        existingUser.IsDeleted = true;

        _dbContext.Users.Update(existingUser);
        var resultOfUpdateUser = await _dbContext.SaveChangesAsync(ct);

        if (resultOfUpdateUser > 0)
        {
            return (AuthMessages.UserDeleteSuccess, AuthMessages.UserDeleteSuccess);
        }

        return AuthErrors.UserDeleteFailure;
    }

    public async Task<ResultT<UserDetailsResponse>> GetProfileAsync(
        Guid userId,
        CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return AuthErrors.UserNotFound;
        }

        var isUserPreferencesCompleted = await _dbContext
        .UserPreferences
        .AnyAsync(x => x.UserId == userId);

        var response = _mapper.Map<UserDetailsResponse>(user);
        response.IsPreferencesCompleted = isUserPreferencesCompleted;

        return (response, AuthMessages.UserFetchedSuccess);
    }
}
