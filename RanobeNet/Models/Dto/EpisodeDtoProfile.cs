using AutoMapper;
using RanobeNet.Models.Data;
using RanobeNet.NovelParser;

namespace RanobeNet.Models.Dto
{
    public class EpisodeDtoProfile : Profile
    {
        public EpisodeDtoProfile()
        {
            CreateMap<Episode, EpisodeDtoForPublic>();
            CreateMap<Episode, EpisodeDtoForMe>();
            CreateMap<Episode, EpisodeDtoForMeList>();
            CreateMap<EpisodeDtoForSave, Episode>();
            var parser = new Parser();
            CreateMap<Episode, EpisodeDtoForPublicParsed>().ForMember(to => to.Story, x => x.MapFrom(from => parser.parse(from.Story)));
        }
    }
}
