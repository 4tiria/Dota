using AutoMapper;
using DotA.API.Helpers;
using DotA.API.Models.EntitiesJs;
using NoSql.Models;

namespace DotA.API.Mappers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Hero, HeroJs>().ReverseMap();
            CreateMap<HeroInMatch, HeroInMatchJs>().ReverseMap();
            
            CreateMap<Match, MatchJs>()
                .ForMember(x => x.Start,
                    dest => dest.MapFrom(
                        src => src.Start.ToLong()))
                .ForMember(x => x.End,
                    dest => dest.MapFrom(
                        src => src.End.ToLong()))
                .ForMember(x => x.DaysAgo,
                    dest => dest.MapFrom(
                        src => GetDaysAgo(src.Start)));
            
            CreateMap<MatchJs, Match>()
                .ForMember(x => x.Start,
                    dest => dest.MapFrom(
                        src => src.Start.ToDateTime()))
                .ForMember(x => x.End,
                    dest => dest.MapFrom(
                        src => src.End.ToDateTime()));

        }

        private int GetDaysAgo(DateTime dateTime) => (int)(DateTime.Now - dateTime).TotalDays;
    }
}