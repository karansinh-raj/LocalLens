using AutoMapper;
using LocalLens.WebApi.Contracts.Questions;
using LocalLens.WebApi.Contracts.UserQuestions;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Errors.UserPreferences;
using LocalLens.WebApi.Errors.UserQuestions;
using LocalLens.WebApi.Messages.Questions;
using LocalLens.WebApi.Messages.UserQuestions;
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
				.OrderBy(x => x.QuestionOrder)
				.ToListAsync();

			var questionsResponse = _mapper.Map<IEnumerable<QuestionOptions>>(questions);
			return (questionsResponse, QuestionsResponseMessages.QuestionsFetchSuccess);
		}

		public async Task<ResultT<string>> CreateUserQuestionsAsync(
			CreateUserQuestionsRequest request,
			Guid userId,
			CancellationToken ct)
		{
			var user = await _dbContext.Users.FindAsync(userId);
			if (user == null)
			{
				return UserPreferencesErrors.UserNotFound;
			}

			var questionIds = request.QuestionsAndOptions.Select(qo => qo.QuestionId).Distinct();
			var optionIds = request.QuestionsAndOptions.Select(qo => qo.OptionId).Distinct();

			var questions = await _dbContext.Questions
				.Where(q => questionIds.Contains(q.Id))
				.ToDictionaryAsync(q => q.Id, q => q);

			var options = await _dbContext.Options
				.Where(o => optionIds.Contains(o.Id))
				.ToDictionaryAsync(o => o.Id, o => o);

            var existingUserQuestions = await
            _dbContext
            .UserQuestions
            .Where(up => up.UserId == userId).ToListAsync();

            foreach (var userQuestion in existingUserQuestions)
            {
				userQuestion.IsDeleted = true;
            }

            _dbContext.UpdateRange(existingUserQuestions);

            var userQuestions = request.QuestionsAndOptions
                .Select(qo => new UserQuestion
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    UserId = userId,
                    QuestionId = qo.QuestionId,
                    Question = questions[qo.QuestionId],
                    OptionId = qo.OptionId,
                    Option = options[qo.OptionId],
                    CreatedOnUtc = DateTime.UtcNow,
                    IsDeleted = false
                })
                .ToList();

			await _dbContext.UserQuestions.AddRangeAsync(userQuestions);
			var resultOfInsert = await _dbContext.SaveChangesAsync();

			if (resultOfInsert > 0)
			{
				return (UserQuestionsMessages.UserQuestionsCreated, UserQuestionsMessages.UserQuestionsCreated);
			}
			return UserQuestionsErrors.UserQuestionsCreateFailure;
		}

		public async Task<ResultT<IEnumerable<UserQuestionsResponse>>> GetAllSelectedQuestionsAsync(Guid userId, CancellationToken ct)
		{
			var questions = await
			_dbContext
			.UserQuestions
			.Where(up => up.UserId == userId && up.IsDeleted == false)
            .Include(m => m.Question)
			.Include(m => m.Option)
			.ToListAsync();

			var groupedQuestions = questions
			.GroupBy(uq => uq.Question)
			.Select(g => new UserQuestionsResponse
			{
				QuestionId = g.Key.Id,
				Options = g.Select(uq => uq.Option.Id).Distinct().ToList()
			})
			.ToList();

			var selectedQuestionsResponse = _mapper.Map<IEnumerable<UserQuestionsResponse>>(groupedQuestions);
			return (selectedQuestionsResponse, QuestionsResponseMessages.QuestionsFetchSuccess);
		}
	}
}
