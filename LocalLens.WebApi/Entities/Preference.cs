namespace LocalLens.WebApi.Entities;

public class Preference : BaseEntity
{
	public Guid Id { get; set; }

	public required string PlaceName { get; set; }
	public required string PreferenceName { get; set; }
}
