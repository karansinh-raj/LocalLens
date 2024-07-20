using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Contracts.Questions
{
	public class QuestionOptions
	{
		public Guid Id { get; set; }
		public required string Text { get; set; }
		public QuestionsType Type { get; set; }
		public required List<OptionResponse> Options { get; set; }
	}
}
