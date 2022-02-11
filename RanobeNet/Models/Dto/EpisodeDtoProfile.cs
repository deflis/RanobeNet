using AutoMapper;
using RanobeNet.Models.Data;

namespace RanobeNet.Models.Dto
{
    public class EpisodeDtoProfile : Profile
    {
        public EpisodeDtoProfile()
        {
            CreateMap<Episode, EpisodeDtoForPublic>();
            CreateMap<Episode, EpisodeDtoForMe>();
            CreateMap<EpisodeDtoForSave, Episode>();
        }
    }
}
