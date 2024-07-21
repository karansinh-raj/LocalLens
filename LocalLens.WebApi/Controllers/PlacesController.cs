using LocalLens.WebApi.Services.Places;
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


    //[HttpGet]
    //public async Task<IActionResult> GetPlaces()
    //{
    //    var response = await _placesService.GetChatResponseAsync();
    //    var places = ParseResponseToPlaces(response);

    //    return Ok(places);
    //}

    //private List<PlaceResponse> ParseResponseToPlaces(string response)
    //{
    //    return JsonConvert.DeserializeObject<List<PlaceResponse>>(response);
    //}
}
