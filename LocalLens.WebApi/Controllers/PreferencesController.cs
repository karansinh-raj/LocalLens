using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Preferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers
{
	[Route("preferences")]
	public class PreferencesController(IPreferencesService preferenceService) : BaseController
	{

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllPreferencesAsync(CancellationToken ct)
		{
			var result = await preferenceService.GetAllPreferencesAsync(ct);

			return result.Match(
				Ok,
				Problem);
		}
	}
}

