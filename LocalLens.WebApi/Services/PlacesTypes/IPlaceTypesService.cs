using LocalLens.WebApi.Contracts.PlacesTypes;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.PlacesTypes;

public interface IPlaceTypesService
{
    Task<ResultT<IEnumerable<PlaceTypeResponse>>> GetPlaceTypes(CancellationToken ct);
}
