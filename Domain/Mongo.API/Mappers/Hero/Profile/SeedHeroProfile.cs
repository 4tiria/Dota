using AutoMapper;
using Domain.Mongo.API.Mappers.Hero.DTO;
using Domain.Mongo.API.Models;

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
            .ForMember(dest => dest.AttackType, opt => opt.MapFrom(src =>
                Enum.Parse<AttackType>(src.attack_type, true)))
            .ForMember(dest => dest.MainAttribute, opt => opt.MapFrom(src =>
                Enum.Parse<MainAttribute>(_attributeMap[src.primary_attr], true)
            ))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.roles));
    }
}