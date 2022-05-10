using AutoMapper;
using RanobeNet.Models.Data;

namespace RanobeNet.Models.Dto
{
    public class ChapterDtoProfile : Profile
    {
        public ChapterDtoProfile()
        {
            CreateMap<Chapter, ChapterDtoForPublic>().ForMember(x => x.Episodes, opt => opt.MapFrom((src) => src.Episodes.OrderBy(x => x.Order)));
            CreateMap<Chapter, ChapterDtoForMeList>().ForMember(x => x.Episodes, opt => opt.MapFrom((src) => src.Episodes.OrderBy(x => x.Order)));
        }
    }
}
