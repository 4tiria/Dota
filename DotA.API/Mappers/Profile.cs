using System;
using AutoMapper;
using DataAccess.Models;
using DotA.API.Models.EntitiesJs;
using HeroInMatch = DataAccess.Models.HeroInMatch;
using Match = DataAccess.Models.Match;

namespace DotA.API.Mappers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Hero, HeroJs>().ReverseMap();
            CreateMap<HeroImage, HeroImageJs>().ReverseMap();
            CreateMap<HeroInMatch, HeroInMatchJs>().ReverseMap();
            CreateMap<Tag, TagJs>().ReverseMap();
            
            CreateMap<Match, MatchJs>()
                .ForMember(x => x.Start,
                    dest => dest.MapFrom(
                        src => src.Start.Ticks))
                .ForMember(x => x.End,
                    dest => dest.MapFrom(
                        src => src.End.Ticks));
            
            CreateMap<MatchJs, Match>()
                .ForMember(x => x.Start,
                    dest => dest.MapFrom(
                        src => new DateTime(src.Start)))
                .ForMember(x => x.End,
                    dest => dest.MapFrom(
                        src => new DateTime(src.End)));

        }
    }
}