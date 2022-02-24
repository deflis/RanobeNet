using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RanobeNet.Models.Dto;
using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Repositories;
using FirebaseAdmin.Auth;
using RanobeNet.Utils;

namespace RanobeNet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public partial class NovelsController : ControllerBase
    {
        private readonly INovelRepository novelRepository;
        private readonly FirebaseAuth firebaseAuth;

        public NovelsController(INovelRepository novelRepository, FirebaseAuth firebaseAuth)
        {
            this.novelRepository = novelRepository;
            this.firebaseAuth = firebaseAuth;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<NovelDtoForPublicListing>>> GetNovels([FromQuery] GetNovelsQueryParams queryParam)
        {
            try
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
                return await novelRepository.GetNovels(builder.build());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NovelDtoForPublic>> Get(long id)
        {
            try
            {
                var novel = await novelRepository.GetNovel(id);

                if (novel == null)
                {
                    return NotFound();
                }
                return novel;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("{id}/me")]
        public async Task<ActionResult<NovelDtoForMe>> GetForMe(long id)
        {
            try
            {
                var firebaseUid = this.HttpContext.GetFirebaseUid();
                var novel = await novelRepository.GetNovelForMe(id, firebaseUid);

                if (novel == null)
                {
                    return NotFound();
                }
                return novel;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<NovelDtoForMe>> Create(NovelDtoForSave novel)
        {
            try
            {
                var firebaseUid = this.HttpContext.GetFirebaseUid();
                return await novelRepository.CreateNovel(firebaseUid, novel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<NovelDtoForMe>> Update(long id, NovelDtoForSave novel)
        {
            try
            {
                var firebaseUid = this.HttpContext.GetFirebaseUid();
                return await novelRepository.UpdateNovel(id, firebaseUid, novel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var firebaseUid = this.HttpContext.GetFirebaseUid();
                await novelRepository.DeleteNovel(id, firebaseUid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
