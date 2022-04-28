using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Models.Dto;

namespace RanobeNet.Repositories
{
    public interface INovelRepository 
    {
        Task<EpisodeDtoForMe> CreateEpisode(long novelId, string firebaseUid, EpisodeDtoForSave episode);
        Task<NovelDtoForMe> CreateNovel(string firebaseUid, NovelDtoForSave novel);
        Task DeleteEpisode(long novelId, long episodeId, string firebaseUid);
        Task DeleteNovel(long id, string firebaseUid);
        Task<ChaptersDto> GetChapters(long id, string firebaseUid);
        Task<EpisodeDtoForPublicParsed> GetEpisode(long novelId, long episodeId);
        Task<EpisodeDtoForMe?> GetEpisodeForMe(long novelId, long episodeId, string firebaseUid);
        Task<NovelDtoForPublic?> GetNovel(long id);
        Task<NovelDtoForMe?> GetNovelForMe(long id, string firebaseUid);
        Task<PagedList<NovelDtoForPublicListing>> GetNovels(Query<Novel> query);
        Task<PagedList<NovelDtoForMe>> GetNovelsByMe(string firebaseUid, Query<Novel> query);
        Task<PagedList<NovelDtoForPublicListing>> GetNovelsByUser(long userId, Query<Novel> query);
        Task<EpisodeDtoForMe> UpdateEpisode(long novelId, long episodeId, string firebaseUid, EpisodeDtoForSave episode);
        Task<NovelDtoForMe> UpdateNovel(long id, string firebaseUid, NovelDtoForSave novel);
        Task UpdateChapters(long id, string firebaseUid, ChaptersDto chapters);
    }
}