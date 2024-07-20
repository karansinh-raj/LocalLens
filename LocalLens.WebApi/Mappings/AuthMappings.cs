using AutoMapper;
using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Mappings;

public class AuthMappings : Profile
{
    public AuthMappings()
    {
        CreateMap<User, UserDetailsResponse>()
            .ReverseMap();

        CreateMap<UpdateUserRequest, User>()
            .ReverseMap();
    }
}
