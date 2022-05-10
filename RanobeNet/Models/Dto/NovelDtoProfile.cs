using AutoMapper;
using RanobeNet.Models.Data;

namespace RanobeNet.Models.Dto
{
    public class NovelDtoProfile : Profile
    {
        public NovelDtoProfile()
        {
            CreateMap<Novel, NovelDtoForPublicListing>().ForMember(dest => dest.Author, src => src.MapFrom(x => x.Author ?? x.User.Name));
            CreateMap<NovelDtoForSave, Novel>();
            CreateMap<NovelLink, NovelLinkDto>();
            CreateMap<NovelLinkDto, NovelLink>();
            CreateMap<NovelTag, NovelTagDto>();
            CreateMap<NovelTagDto, NovelTag>();
        }
    }
}
