using LocalLens.WebApi.Contracts.Questions;
using LocalLens.WebApi.Contracts.UserPreferences;
using LocalLens.WebApi.Contracts.UserQuestions;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.Questions
{
	public interface IQuestionsService
	{
		Task<ResultT<IEnumerable<QuestionOptions>>> GetAllQuestionsAsync(CancellationToken ct);

        Task<ResultT<string>> CreateUserQuestionsAsync(
            CreateUserQuestionsRequest request,
            Guid userId,
            CancellationToken ct);
    }
}
