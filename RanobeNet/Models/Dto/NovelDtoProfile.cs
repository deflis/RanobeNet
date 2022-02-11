using AutoMapper;
using RanobeNet.Models.Data;

namespace RanobeNet.Models.Dto
{
    public class NovelDtoProfile : Profile
    {
        public NovelDtoProfile()
        {
            CreateMap<Novel, NovelDtoForMe>();
            CreateMap<Novel, NovelDtoForPublicListing>();
            CreateMap<NovelDtoForSave, Novel>();
        }
    }
}
