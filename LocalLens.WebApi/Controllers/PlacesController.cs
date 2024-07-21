using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.Services.Places;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers;

[Route("places")]
public class PlacesController : BaseController
{
    private readonly IPlacesService _placesService;

    public PlacesController(IPlacesService placesService)
    {
        _placesService = placesService;
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPlaces(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var response = await _placesService.GetChatResponseAsync(userId, ct);
        return Ok(response);
    }
}
