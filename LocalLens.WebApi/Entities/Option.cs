using System.ComponentModel.DataAnnotations.Schema;

namespace LocalLens.WebApi.Entities;
public class Option : BaseEntity
{
	public Guid Id { get; set; }
	public required string Text { get; set; }

	public Guid QuestionId { get; set; }

	[ForeignKey(nameof(QuestionId))]
	public Question Question { get; set; } = null!;
}
