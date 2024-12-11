using AutoMapper;
using Domain.Mongo.API.Models;
using Dota.API.Helpers;
using Dota.API.Models.EntitiesJs;
using MongoDB.Bson;

namespace Dota.API.Mappers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Domain.Mongo.API.Models.Hero, HeroJs>().ReverseMap();
        CreateMap<HeroInMatch, HeroInMatchJs>().ReverseMap();
        
        _ = CreateMap<Match, MatchJs>()
            .ForMember(x => x.Id, 
                dest => dest.MapFrom(
                    src => src.Id.ToString()))
            .ForMember(x => x.Start,
                dest => dest.MapFrom(
                    src => src.Start.ToLong()))
            .ForMember(x => x.End,
                dest => dest.MapFrom(
                    src => src.End.ToLong()))
            .ForMember(x => x.DaysAgo,
                dest => dest.MapFrom(
                    src => GetDaysAgo(src.Start)));
        
        _ = CreateMap<MatchJs, Match>()
             .ForMember(x => x.Id,
                dest => dest.MapFrom(
                    src => new ObjectId(src.Id)))
            .ForMember(x => x.Start,
                dest => dest.MapFrom(
                    src => src.Start.ToDateTime()))
            .ForMember(x => x.End,
                dest => dest.MapFrom(
                    src => src.End.ToDateTime()));
    }

    private int GetDaysAgo(DateTime dateTime) => (int)(DateTime.Now - dateTime).TotalDays;
}