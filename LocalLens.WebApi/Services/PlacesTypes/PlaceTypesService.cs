using AutoMapper;
using LocalLens.WebApi.Contracts.PlacesTypes;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Messages.PlaceTypes;
using LocalLens.WebApi.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace LocalLens.WebApi.Services.PlacesTypes;

public class PlaceTypesService : IPlaceTypesService
{
    private readonly LocalLensDbContext _dbContext;
    private readonly IMapper _mapper;

    public PlaceTypesService(
        LocalLensDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultT<IEnumerable<PlaceTypeResponse>>> GetPlaceTypes(CancellationToken ct)
    {
        var places = await
            _dbContext.
            PlaceTypes.
            ToListAsync();

        var placesResponse = _mapper.Map<IEnumerable<PlaceTypeResponse>>(places);
        return (placesResponse, PlaceTypesMessages.PlacesFetchSuccess);
    }
}
