namespace LocalLens.WebApi.Entities;

public class PlaceType : BaseEntity
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
}
