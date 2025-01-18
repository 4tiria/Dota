using AutoMapper;
using Domain.Mongo.API.Mappers.Hero.DTO;

namespace Domain.Mongo.API.Mappers;

public class SeedHeroProfile : Profile
{
    private readonly Dictionary<string, string> _attributeMap = new()
    {
        { "agi", "agility" },
        { "int", "intelligence" },
        { "str", "strength" },
        { "all", "universal" }
    };

    public SeedHeroProfile()
    {
        CreateMap<SeedHeroDto, API.Hero>()
            .ForMember(dest => dest.UnderscoreName, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.LocalizedName, opt => opt.MapFrom(src => src.localized_name))
            .ForMember(dest => dest.AttackType, opt => opt.MapFrom(src => src.attack_type))
            .ForMember(dest => dest.MainAttribute, opt => opt.MapFrom(src =>
                _attributeMap[src.primary_attr]
            ))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.roles));
    }
}