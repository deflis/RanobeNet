using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Models.Dto;

namespace RanobeNet.Repositories
{
    public class NovelRepository : INovelRepository
    {
        private RanobeNetContext context;
        private IMapper mapper;

        public NovelRepository(RanobeNetContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Task<PagedList<NovelDtoForPublicListing>> GetNovels(Query<Novel> query)
        {
            return context.Novels.ToPagedListAsync<Novel, NovelDtoForPublicListing>(query, mapper);
        }

        public Task<PagedList<NovelDtoForPublicListing>> GetNovelsByUser(long userId, Query<Novel> query)
        {
            return context.Novels.Where(x => x.UserId == userId).ToPagedListAsync<Novel, NovelDtoForPublicListing>(query, mapper);
        }

        async public Task<NovelDtoForPublic?> GetNovel(long id)
        {
            var novel = await context.Novels.FindAsync(id);
            if (novel == null) return null;

            return new NovelDtoForPublic
            {
                Id = novel.Id,
                Title = novel.Title,
                Description = novel.Description,
                Author = novel.Author ?? novel.User.Name,
                Chapters = new List<ChapterDtoForPublic>(){
                    new ChapterDtoForPublic
                    {
                        Type = ChapterType.NonChapter,
                        Episodes = novel.Episodes.Where(x => x.ChapterId == null).OrderBy(x => x.Order).Select(x => mapper.Map<EpisodeDtoForPublic>(x))
                    }
                }.Concat(novel.Chapters.OrderBy(x => x.Order).Select(chapter => new ChapterDtoForPublic
                {
                    Type = ChapterType.Chapter,
                    Title = chapter.Title,
                    Episodes = novel.Episodes.Where(x => x.ChapterId == chapter.Id).OrderBy(x => x.Order).Select(x => mapper.Map<EpisodeDtoForPublic>(x))
                })),
            };
        }

        async public Task<NovelDtoForMe?> GetNovelForMe(long id, string firebaseUid)
        {
            var rawNovel = await context.Novels.FindAsync(id);
            if (rawNovel == null) return null;
            if (rawNovel.User.FirebaseUid != firebaseUid) throw new Exception();
            return mapper.Map<NovelDtoForMe>(rawNovel);
        }

        async public Task<NovelDtoForMe> CreateNovel(string firebaseUid, NovelDtoForSave novel)
        {
            var userId = await context.Users.Where(x => x.FirebaseUid == firebaseUid).Select(x => x.Id).SingleAsync();
            var rawNovel = mapper.Map<Novel>(novel);
            rawNovel.UserId = userId;
            await context.AddAsync(rawNovel);
            await context.SaveChangesAsync();
            return mapper.Map<NovelDtoForMe>(rawNovel);
        }

        async public Task<NovelDtoForMe> UpdateNovel(long id, string firebaseUid, NovelDtoForSave novel)
        {
            var rawNovel = await context.Novels.FindAsync(id);
            if (rawNovel == null) throw new Exception();
            if (rawNovel.User.FirebaseUid != firebaseUid) throw new Exception();
            rawNovel.Title = novel.Title;
            rawNovel.Description = novel.Description;
            rawNovel.Author = novel.Author;
            await context.SaveChangesAsync();
            return mapper.Map<NovelDtoForMe>(rawNovel);
        }

        async public Task DeleteNovel(long id, string firebaseUid)
        {
            var userId = await context.Users.Where(x => x.FirebaseUid == firebaseUid).Select(x => x.Id).SingleAsync();
            var rawNovel = await context.Novels.SingleAsync(x => x.Id == id && x.UserId == userId);
            context.Novels.Remove(rawNovel);
            await context.SaveChangesAsync();
        }
        async public Task<EpisodeDtoForMe?> GetEpisode(long novelId, long episodeId, string firebaseUid)
        {
            var episode = await context.Episodes.FindAsync(episodeId);
            if (episode == null) throw new Exception();
            if (episode.Novel.Id != novelId) throw new Exception();
            if (episode.Novel.User.FirebaseUid != firebaseUid) throw new Exception();
            return mapper.Map<EpisodeDtoForMe>(episode);
        }

        async public Task<EpisodeDtoForMe> CreateEpisode(long novelId, string firebaseUid, EpisodeDtoForSave episode)
        {
            var novel = await context.Novels.FindAsync(novelId);
            if (novel == null) throw new Exception();
            if (novel.User.FirebaseUid != firebaseUid) throw new Exception();
            var rawEpisode = mapper.Map<Episode>(episode);
            rawEpisode.NovelId = novelId;
            rawEpisode.Order = ((await context.Episodes.Where(x => x.NovelId == novel.Id).MaxAsync(x => (int?)x.Order)) ?? 0) + 1;
            rawEpisode.ChapterId = await context.Episodes.Where(x => x.NovelId == novel.Id).OrderByDescending(x => x.Order).Select(x => x.ChapterId).FirstOrDefaultAsync();
            context.Episodes.Add(rawEpisode);
            await context.SaveChangesAsync();
            return mapper.Map<EpisodeDtoForMe>(rawEpisode);
        }

        async public Task<EpisodeDtoForMe> UpdateEpisode(long novelId, long episodeId, string firebaseUid, EpisodeDtoForSave episode)
        {
            var rawEpisode = await context.Episodes.FindAsync(episodeId);
            if (rawEpisode == null) throw new Exception();
            if (rawEpisode.Novel.Id != novelId) throw new Exception();
            if (rawEpisode.Novel.User.FirebaseUid != firebaseUid) throw new Exception();
            rawEpisode.Title = episode.Title;
            rawEpisode.Story = episode.Story;
            await context.SaveChangesAsync();
            return mapper.Map<EpisodeDtoForMe>(rawEpisode);
        }

        async public Task DeleteEpisode(long novelId, long episodeId, string firebaseUid)
        {
            var episode = await context.Episodes.FindAsync(episodeId);
            if (episode == null) throw new Exception();
            if (episode.Novel.Id != novelId) throw new Exception();
            if (episode.Novel.User.FirebaseUid != firebaseUid) throw new Exception();
            context.Episodes.Remove(episode);
            await context.Episodes
                    .Where(x => x.NovelId == novelId && x.Id != episodeId)
                    .OrderBy(x => x.Order)
                    .Zip(Enumerable.Range(1, int.MaxValue), (episode, order) => new { episode, order })
                    .ForEachAsync(x =>
                    {
                        x.episode.Order = x.order;
                    });
            await context.SaveChangesAsync();

        }
    }
}

