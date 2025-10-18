using AutoMapper;
using Domain.Mongo.API.Models;
using Dota.API.Account.DTO;
using Dota.API.Commands.CreateAccount;
using Dota.API.Helpers;
using Dota.API.Models.EntitiesJs;

namespace Dota.API.Mappers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Domain.Mongo.API.Hero, HeroJs>()
            .ForMember(hero => hero.Name, dest => dest.MapFrom(src => src.LocalizedName));

        CreateMap<HeroInMatch, HeroInMatchJs>();

        CreateMap<Match, MatchJs>()
            .ForMember(matchJs => matchJs.Id,
                dest => dest.MapFrom(
                    src => src.Id.ToString()))
            .ForMember(matchJs => matchJs.Start,
                dest => dest.MapFrom(
                    src => src.Start.ToLong()))
            .ForMember(matchJs => matchJs.End,
                dest => dest.MapFrom(
                    src => src.End.ToLong()))
            .ForMember(matchJs => matchJs.DaysAgo,
                dest => dest.MapFrom(
                    src => GetDaysAgo(src.Start)));

        CreateMap<CreateAccountRequest, Domain.Mongo.API.Account>();
        CreateMap<AccountCreated, CreateAccountRequest>();
    }

    private int GetDaysAgo(DateTime dateTime)
    {
        return (int)(DateTime.Now - dateTime).TotalDays;
    }
}