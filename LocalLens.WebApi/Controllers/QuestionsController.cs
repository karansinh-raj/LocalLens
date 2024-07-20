using LocalLens.WebApi.Contracts.UserPreferences;
using LocalLens.WebApi.Contracts.UserQuestions;
using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Questions;
using LocalLens.WebApi.Services.UserPreferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers
{
	[Route("questions")]
	public class QuestionsController(IQuestionsService questionsService) : BaseController
	{
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllQuestionsAsync(CancellationToken ct)
		{
			var result = await questionsService.GetAllQuestionsAsync(ct);

			return result.Match(
				Ok,
				Problem);
		}

        [HttpPost("/questions/user")]
        [Authorize]
        public async Task<IActionResult> CreateUserQuestionAsync(
        CreateUserQuestionsRequest request,
            CancellationToken ct)
        {
            var userId = User.GetUserId();
            var result = await questionsService.CreateUserQuestionsAsync(request, userId, ct);

            return result.Match(
                Ok,
                Problem);
        }
    }
}
