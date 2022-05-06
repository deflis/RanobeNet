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

        public Task<PagedList<NovelDtoForMe>> GetNovelsByMe(string firebaseUid, Query<Novel> query)
        {
            return context.Novels.Where(x => x.User.FirebaseUid == firebaseUid).ToPagedListAsync<Novel, NovelDtoForMe>(query, mapper);
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
                UserId = novel.UserId,
                Chapters = new List<ChapterDtoForPublic>() {
                    new ChapterDtoForPublic
                    {
                        Type = ChapterType.NonChapter,
                        Episodes = novel.Episodes.Where(x => x.ChapterId == null).OrderBy(x => x.Order).Select(x => mapper.Map<EpisodeDtoForPublic>(x))
                    }
                }.Concat(novel.Chapters.OrderBy(x => x.Order).Where(chapter => chapter.Episodes.Count > 0).Select(chapter => new ChapterDtoForPublic
                {
                    Type = ChapterType.Chapter,
                    Title = chapter.Title,
                    Episodes = novel.Episodes.Where(x => x.ChapterId == chapter.Id).OrderBy(x => x.Order).Select(x => mapper.Map<EpisodeDtoForPublic>(x))
                })),
                Links = novel.Links.Select(x => mapper.Map<NovelLinkDto>(x)),
                Tags = novel.Tags.Select(x => mapper.Map<NovelTagDto>(x))
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
            rawNovel.Links = novel.Links.Select(x => mapper.Map<NovelLink>(x)).ToList();
            rawNovel.Tags = novel.Tags.Select(x => mapper.Map<NovelTag>(x)).ToList();
            await context.AddAsync(rawNovel);
            await context.AddRangeAsync(novel.Links.Select(x => new NovelLink
            {
                NovelId = rawNovel.Id,
                Link = x.Link,
                Name = x.Name,
            }));
            await context.AddRangeAsync(novel.Tags.Select(x => new NovelTag
            {
                NovelId = rawNovel.Id,
                Tag = x.Tag,
            }));
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
            rawNovel.Links = novel.Links.Select(x => mapper.Map<NovelLink>(x)).ToList();
            rawNovel.Tags = novel.Tags.Select(x => mapper.Map<NovelTag>(x)).ToList();
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

        async public Task<EpisodeDtoForPublicParsed> GetEpisode(long novelId, long episodeId)
        {
            var episode = await context.Episodes.FindAsync(episodeId);
            if (episode == null) throw new Exception();
            if (episode.Novel.Id != novelId) throw new Exception();
            return mapper.Map<EpisodeDtoForPublicParsed>(episode);
        }

        async public Task<EpisodeDtoForMe?> GetEpisodeForMe(long novelId, long episodeId, string firebaseUid)
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
            await context.SaveChangesAsync();
        }

        async public Task<ChaptersDto> GetChapters(long id, string firebaseUid)
        {
            var novel = await context.Novels.FindAsync(id);
            if (novel == null) return null;
            if (novel.User.FirebaseUid != firebaseUid) throw new Exception();

            return new ChaptersDto()
            {
                Chapters = new List<ChaptersDto.Chapter> {
                    new ChaptersDto.Chapter
                    {
                        Type = ChapterType.NonChapter,
                        Episodes = novel.Episodes.Where(x => x.ChapterId == null).OrderBy(x => x.Order).Select(x => new ChaptersDto.Episode
                        {
                            Id = x.Id,
                        })
                    }
                }.Concat(novel.Chapters.OrderBy(x => x.Order).Where(chapter => chapter.Episodes.Count > 0).Select(chapter =>
                new ChaptersDto.Chapter
                {
                    Id = chapter.Id,
                    Type = ChapterType.Chapter,
                    Title = chapter.Title,
                    Episodes = novel.Episodes.Where(x => x.ChapterId == chapter.Id).OrderBy(x => x.Order).Select(x => new ChaptersDto.Episode
                    {
                        Id = x.Id,
                    }),
                }))

            };
        }

        async public Task UpdateChapters(long id, string firebaseUid, ChaptersDto chapters)
        {
            var novel = await context.Novels.FindAsync(id);
            if (novel == null || novel.User.FirebaseUid != firebaseUid) throw new Exception();

            // 消えたチャプターを削除する
            var existChapterId = chapters.Chapters.Select(x => x.Id);
            var deletedChapters = novel.Chapters.Where(x => !existChapterId.Contains(x.Id));
            context.Chapters.RemoveRange(deletedChapters);

            var chaptersWithIndex = chapters.Chapters.Where(x => x.Type == ChapterType.Chapter).Select((x, i) => new
            {
                Order = i + 1,
                Id = x.Id,
                Title = x.Title
            });
            // 既存分更新
            var chapterIdsWithIndex = chaptersWithIndex.Where(x => x.Id != null).Select(x => x.Id);
            foreach (var chapter in novel.Chapters.Where(x => chapterIdsWithIndex.Contains(x.Id)))
            {
                var update = chaptersWithIndex.Single(x => x.Id == chapter.Id);
                chapter.Title = update.Title!;
                chapter.Order = update.Order;
            }
            // 新規分
            var newChapters = chaptersWithIndex.Where(x => x.Id == null).Select(x =>
                new Chapter
                {
                    NovelId = id,
                    Order = x.Order,
                    Title = x.Title,
                }
            );
            await context.Chapters.AddRangeAsync(newChapters);
            await context.SaveChangesAsync();

            // エピソード順と所属チャプター更新（チャプター更新は不完全）
            var episodesWithIndex = chapters.Chapters.SelectMany(x => x.Episodes.Select(y => new { EpisodeId = y.Id, ChapterId = x.Id }).Select((x, i) => new
            {
                Order = i + 1,
                Id = x.EpisodeId,
                ChapterId = x.ChapterId
            }));
            foreach (var episode in novel.Episodes)
            {
                var update = episodesWithIndex.Single(x => x.Id == episode.Id);
                episode.Order = update.Order;
                episode.ChapterId = update.ChapterId;
            }

            await context.SaveChangesAsync();
        }
    }
}

