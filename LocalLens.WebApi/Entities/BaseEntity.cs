namespace LocalLens.WebApi.Entities;

public class BaseEntity : IBaseEntity
{
    public DateTime CreatedOnUtc { get; set; }
    public DateTime UpdatedOnUtc { get; set; }
    public DateTime DeletedOnUtc { get; set; }
    public bool IsDeleted { get; set; }
}
