namespace LocalLens.WebApi.Contracts.UserPreferences;

public class UserPreferencesResponse
{
	public Guid Id { get; set; }
	public required string PlaceName { get; set; }
	public required string PreferenceName { get; set; }
}
