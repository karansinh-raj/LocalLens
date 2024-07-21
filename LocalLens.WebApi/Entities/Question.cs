namespace LocalLens.WebApi.Entities;

public class Question : BaseEntity
{
	public Guid Id { get; set; }
	public QuestionsType questionsType { get; set; }
	public required string Text { get; set; }
	public bool IsMultiChoice { get; set; }
	public bool IsDropDown { get; set; }
	public ICollection<Option> Options { get; set; } = [];
}