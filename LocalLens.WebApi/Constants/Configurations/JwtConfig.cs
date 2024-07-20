namespace LocalLens.WebApi.Constants.Configurations;

public class JwtConfig
{
    public const string Key = "JwtSettings";

    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SecretKey { get; set; }
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
}
