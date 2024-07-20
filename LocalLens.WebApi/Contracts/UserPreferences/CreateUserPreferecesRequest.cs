namespace LocalLens.WebApi.Contracts.UserPreferences;

public class CreateUserPreferecesRequest
{
    public IEnumerable<Guid> Preferences { get; set; }
}
