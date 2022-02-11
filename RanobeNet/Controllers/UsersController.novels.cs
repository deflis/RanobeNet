#nullable disable
using Microsoft.AspNetCore.Mvc;
using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Models.Dto;

namespace RanobeNet.Controllers
{
    public partial class UsersController
    {
        [HttpGet("{id}/novels")]
        public async Task<ActionResult<PagedList<NovelDtoForPublicListing>>> GetNovelsByUser(long id, [FromQuery] GetNovelsQueryParams queryParam)
        {
            var builder = QueryBuilder<Novel>.create(queryParam.page, queryParam.size).SetDescending(queryParam.descending);
            switch (queryParam.order)
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
            return await novelRepository.GetNovels(builder.build());
        }
    }
}
