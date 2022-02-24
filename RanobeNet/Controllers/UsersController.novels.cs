#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Models.Dto;
using RanobeNet.Utils;

namespace RanobeNet.Controllers
{
    public partial class UsersController
    {
        [HttpGet("{id}/novels")]
        public async Task<ActionResult<PagedList<NovelDtoForPublicListing>>> GetNovelsByUser(long id, [FromQuery] GetNovelsQueryParams queryParam)
        {
            var builder = QueryBuilder<Novel>.create(queryParam.page ?? 1, queryParam.size ?? 10).SetDescending(queryParam.descending ?? false);
            switch (queryParam.order ?? NovelField.id)
            {
                case NovelField.id:
                    builder.SetKeySelector(x => x.Id);
                    break;
                case NovelField.title:
                    builder.SetKeySelector(x => x.Title);
                    break;
                case NovelField.created:
                    builder.SetKeySelector(x => x.CreatedAt);
                    break;
                case NovelField.modified:
                    builder.SetKeySelector(x => x.UpdatedAt);
                    break;
            }
            return await novelRepository.GetNovelsByUser(id, builder.build());
        }

        [Authorize]
        [HttpGet("me/novels")]
        public async Task<ActionResult<PagedList<NovelDtoForMe>>> GetNovelsByMe([FromQuery] GetNovelsQueryParams queryParam)
        {
            var firebaseUid = this.HttpContext.GetFirebaseUid();
            var builder = QueryBuilder<Novel>.create(queryParam.page ?? 1, queryParam.size ?? 10).SetDescending(queryParam.descending ?? false);
            switch (queryParam.order ?? NovelField.id)
            {
                case NovelField.id:
                    builder.SetKeySelector(x => x.Id);
                    break;
                case NovelField.title:
                    builder.SetKeySelector(x => x.Title);
                    break;
                case NovelField.created:
                    builder.SetKeySelector(x => x.CreatedAt);
                    break;
                case NovelField.modified:
                    builder.SetKeySelector(x => x.UpdatedAt);
                    break;
            }
            return await novelRepository.GetNovelsByMe(firebaseUid, builder.build());
        }
    }
}
