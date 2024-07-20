using AutoMapper;
using LocalLens.WebApi.Contracts.Preferences;
using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Preference, PreferenceResponse>()
			.ReverseMap();

	}
}
