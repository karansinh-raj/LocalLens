using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers
{
	[Route("questions")]
	public class QuestionsController(IQuestionsService questionsService) : BaseController
	{
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllPreferencesAsync(CancellationToken ct)
		{
			var result = await questionsService.GetAllQuestionsAsync(ct);

			return result.Match(
				Ok,
				Problem);
		}
	}
}
