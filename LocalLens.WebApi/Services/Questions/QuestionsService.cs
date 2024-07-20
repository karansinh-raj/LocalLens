using AutoMapper;
using LocalLens.WebApi.Contracts.Questions;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Messages.Questions;
using LocalLens.WebApi.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace LocalLens.WebApi.Services.Questions
{
	public class QuestionsService(IMapper _mapper, LocalLensDbContext _dbContext) : IQuestionsService
	{
		public async Task<ResultT<IEnumerable<QuestionOptions>>> GetAllQuestionsAsync(CancellationToken ct)
		{
			var questions = await
				_dbContext
				.Questions.Include(m => m.Options)
				.ToListAsync();

			var questionsResponse = _mapper.Map<IEnumerable<QuestionOptions>>(questions);
			return (questionsResponse, QuestionsResponseMessages.QuestionsFetchSuccess);
		}
	}
}
