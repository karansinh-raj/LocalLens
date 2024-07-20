using AutoMapper;
using LocalLens.WebApi.Contracts.PlacesTypes;
using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Mappings;

public class PlaceTypeMappings : Profile
{
    public PlaceTypeMappings()
    {
        CreateMap<PlaceType, PlaceTypeResponse>()
            .ReverseMap();
    }
}
