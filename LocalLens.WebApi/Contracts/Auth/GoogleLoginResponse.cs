namespace LocalLens.WebApi.Contracts.Auth;

public class GoogleLoginResponse
{
    public string AccessToken { get; set; }
    public DateTime ExpiryTimeUtc { get; set; }
    public bool IsPreferencesCompleted { get; set; }
}
