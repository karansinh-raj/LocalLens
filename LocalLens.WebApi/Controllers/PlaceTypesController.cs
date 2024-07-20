using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.PlacesTypes;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers;

[Route("place-types")]
public class PlaceTypesController : BaseController
{
    private readonly IPlaceTypesService _placeTypesService;

    public PlaceTypesController(
        IPlaceTypesService placeTypesService)
    {
        _placeTypesService = placeTypesService;
    }

    [HttpGet]
    public async Task<IActionResult> GoogleLoginAsync(
        CancellationToken ct)
    {
        var result = await _placeTypesService.GetPlaceTypes(ct);

        return result.Match(
            Ok,
            Problem);
    }
}
