using AutoMapper;
using Domain.Mongo.API.Models;
using Dota.API.Account.DTO;
using Dota.API.Commands.CreateAccount;
using Dota.API.Commands.UpdateAccountFeed;
using Dota.API.Helpers;
using Dota.API.Infrastructure.WebSocket.Account;
using Dota.API.Models.EntitiesJs;
using Dota.API.Queries.GetAllAccounts;

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
        CreateMap<AccountCreated, UpdateAccountFeedNotification>();
        CreateMap<Domain.Mongo.API.Account, AccountDto>();
        CreateMap<UpdateAccountFeedDto, UpdateAccountFeedNotification>();
    }

    private int GetDaysAgo(DateTime dateTime)
    {
        return (int)(DateTime.Now - dateTime).TotalDays;
    }
}