using Microsoft.AspNetCore.Identity;

namespace LocalLens.WebApi.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfileUrl { get; set; }

    public LoginProvider LoginProvider { get; set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime UpdatedOnUtc { get; set; }
    public DateTime DeletedOnUtc { get; set; }
    public bool IsDeleted { get; set; } = false;
}
