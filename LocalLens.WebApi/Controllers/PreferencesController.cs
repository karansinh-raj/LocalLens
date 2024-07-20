using LocalLens.WebApi.Contracts.UserPreferences;
using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Preferences;
using LocalLens.WebApi.Services.UserPreferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers
{
	[Route("preferences")]
	public class PreferencesController(
		IPreferencesService preferenceService,
		IUserPreferencesService userPreferencesService) : BaseController
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

        [HttpPost("/preferences/user")]
        [Authorize]
        public async Task<IActionResult> CreateUserPreferencesAsync(
			CreateUserPreferecesRequest request,
			CancellationToken ct)
        {
			var userId = User.GetUserId();
            var result = await userPreferencesService.CreateUserPreferencesAsync(request, userId, ct);

            return result.Match(
                Ok,
                Problem);
        }
    }
}

