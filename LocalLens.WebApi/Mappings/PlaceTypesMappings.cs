using AutoMapper;
using LocalLens.WebApi.Contracts.PlacesTypes;
using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Mappings;

public class PlaceTypesMappings : Profile
{
    public PlaceTypesMappings()
    {
        CreateMap<PlaceType, PlaceTypeResponse>()
            .ReverseMap();
    }
}
