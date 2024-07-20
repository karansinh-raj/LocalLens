using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Contracts.Auth;

public class UserDetailsResponse
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileUrl { get; set; }
    public string Email { get; set; }
    public LoginProvider LoginProvider { get; set; }
}
