using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.Services.UserPredicatedPlaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers;

[Route("predicated-places")]
public class UserPredicatedPlacesControllers : BaseController
{
    private readonly IUserPredicatedPlaces _userPredicatedPlaces;

    public UserPredicatedPlacesControllers(IUserPredicatedPlaces userPredicatedPlaces)
    {
        _userPredicatedPlaces = userPredicatedPlaces;
    }

    //[HttpGet]
    //[Authorize]
    //public async Task<IActionResult> GetProfileAsync(
    //    CancellationToken ct)
    //{
    //    var userId = User.GetUserId();
    //    //var result = await _userPredicatedPlaces;

    //    //return result.Match(
    //    //    Ok,
    //    //    Problem);
    //}
}
