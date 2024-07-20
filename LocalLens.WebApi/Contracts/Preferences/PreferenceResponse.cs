namespace LocalLens.WebApi.Contracts.Preferences
{
	public class PreferenceResponse
	{
		public Guid Id { get; set; }

		public required string PlaceName { get; set; }
		public required string PreferenceName { get; set; }
	}
}
