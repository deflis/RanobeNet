#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RanobeNet.Models.Dto;
using RanobeNet.Utils;

namespace RanobeNet.Controllers
{
    public partial class NovelsController
    {
        [Authorize]
        [HttpGet("{id}/chapters")]
        public async Task<ActionResult<ChaptersDtoForMe>> GetChapters(long id)
        {
            var firebaseUid = this.HttpContext.GetFirebaseUid();
            return await novelRepository.GetChapters(id, firebaseUid);
        }

        [Authorize]
        [HttpPost("{id}/chapters")]
        public async Task<ActionResult> UpdateChapters(long id, ChaptersDtoForSave chapters)
        {
            var firebaseUid = this.HttpContext.GetFirebaseUid();
            await novelRepository.UpdateChapters(id, firebaseUid, chapters);
            return Ok();
        }
    }
}
