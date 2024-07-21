namespace LocalLens.WebApi.Contracts.UserQuestions;

public class UserQuestionsResponse
{
	public Guid QuestionId { get; set; }
	public IEnumerable<Guid> Options { get; set; }
}
