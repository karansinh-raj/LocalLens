namespace LocalLens.WebApi.Entities;

public class Preference : BaseEntity
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
}
