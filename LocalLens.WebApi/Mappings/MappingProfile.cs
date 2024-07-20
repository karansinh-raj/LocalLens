using AutoMapper;
using LocalLens.WebApi.Contracts.Preferences;
using LocalLens.WebApi.Contracts.Questions;
using LocalLens.WebApi.Entities;

namespace LocalLens.WebApi.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Preference, PreferenceResponse>()
			.ReverseMap();
		CreateMap<Question, QuestionOptions>()
            .ForMember(x => x.Type, src => src.MapFrom(src => src.questionsType))
            .ReverseMap();
		CreateMap<Option, OptionResponse>()
			.ReverseMap();
	}
}
