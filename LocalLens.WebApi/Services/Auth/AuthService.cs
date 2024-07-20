using Google.Apis.Auth;
using LocalLens.WebApi.Constants.Configurations;
using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Errors.Auth;
using LocalLens.WebApi.Messages.Auth;
using LocalLens.WebApi.ResultPattern;
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
    private readonly GoogleAuthConfig _googleAuthConfig;
    private readonly JwtConfig _jwtConfig;

    public AuthService(
        ILogger<AuthService> logger, 
        IOptions<GoogleAuthConfig> googleAuthConfig,
        IOptions<JwtConfig> jwtConfig,
        LocalLensDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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

        var resultOfCreateUser = await
            CreateUserIfNotExists(userInfo.Email, userInfo.FirstName, userInfo.LastName, userInfo.ProfileUrl);

        if (!resultOfCreateUser)
        {
            return AuthErrors.UserCreateFailure;
        }

        var claims = GetClaims(userInfo);
        var (accessToken, accessTokenExpiry) = GenerateAccessToken(claims);

        var response = new GoogleLoginResponse
        {
            AccessToken = accessToken,
            ExpiryTimeUtc = accessTokenExpiry
        };

        return (new GoogleLoginResponse(), AuthMessages.LoginSuccess);
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

    private async Task<bool> CreateUserIfNotExists(
        string email,
        string firstName,
        string lastName,
        string profileUrl)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            ProfileUrl = profileUrl,
            LoginProvider = LoginProvider.Google,
            CreatedOnUtc = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user);
        var resultOfCreateUser = await _dbContext.SaveChangesAsync();
        return resultOfCreateUser > 0;
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
       GoogleUserDetailsResponse user) =>
       [
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
           new Claim(JwtRegisteredClaimNames.Email, user.Email!.ToString()),
           new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName!.ToString()),
           new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName!.ToString()),
       ];
}
