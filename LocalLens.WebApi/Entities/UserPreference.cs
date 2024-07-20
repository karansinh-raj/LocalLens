using System.ComponentModel.DataAnnotations.Schema;

namespace LocalLens.WebApi.Entities;

public class UserPreference : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public Guid PreferenceId { get; set; }

    [ForeignKey(nameof(PreferenceId))]
    public Preference Preference { get; set; } = null!;
}
