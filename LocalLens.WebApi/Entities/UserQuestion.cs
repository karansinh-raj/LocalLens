using System.ComponentModel.DataAnnotations.Schema;

namespace LocalLens.WebApi.Entities;

public class UserQuestion : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public Guid QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; } = null!;

    public Guid OptionId { get; set; }

    [ForeignKey(nameof(OptionId))]
    public Option Option { get; set; } = null!;
}
