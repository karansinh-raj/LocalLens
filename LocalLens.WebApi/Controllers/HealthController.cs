using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers;

[Route("health")]
public class HealthController : BaseController
{
	[HttpGet]
	public async Task<IActionResult> GetHealthAsync()
	{
		return await Task.FromResult(Ok());
	}
}
