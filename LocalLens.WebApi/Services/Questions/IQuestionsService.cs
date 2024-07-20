using LocalLens.WebApi.Contracts.Questions;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.Questions
{
	public interface IQuestionsService
	{
		Task<ResultT<IEnumerable<QuestionOptions>>> GetAllQuestionsAsync(CancellationToken ct);
	}
}
