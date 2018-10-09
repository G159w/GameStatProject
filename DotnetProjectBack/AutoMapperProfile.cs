using AutoMapper;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models.ApiResponses;
using Models.Requests;
using Models.Responses;
using System.Collections.Generic;

namespace DotnetProjectBack
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TUser, UserResponse>()
                .ForMember(d => d.Games, opt => opt.MapFrom(s => Mapper.Map<IEnumerable<TUserGame>, IEnumerable<UserGameResponse>>(s.TUserGame)))
                .ForMember(d => d.Friends, opt => opt.MapFrom(s => Mapper.Map<IEnumerable<TFriend>, IEnumerable<FriendResponse>>(s.TFriendUser)));

            CreateMap<TUserGame, UserGameResponse>()
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(src => src.Game.DisplayName))
                .ForMember(d => d.ShortName, opts => opts.MapFrom(src => src.Game.ShortName))
                .ForMember(d => d.Username, opts => opts.MapFrom(src => src.Username));

            CreateMap<TFriend, FriendResponse>()
               .ForMember(d => d.Username, opts => opts.MapFrom(src => src.Friend.Username))
               .ForMember(d => d.Games, opts => opts.MapFrom(s => Mapper.Map<IEnumerable<TUserGame>, IEnumerable<UserGameResponse>>(s.Friend.TUserGame)))
               .ForMember(d => d.RegisterDate, opts => opts.MapFrom(src => src.Friend.RegisterDate))
               .ForMember(d => d.Id, opts => opts.MapFrom(src => src.Friend.Id));

            CreateMap<TGame, BaseGameResponse>();
        }
    }
}
