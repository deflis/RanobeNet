#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RanobeNet.Models.Dto;
using RanobeNet.Utils;

namespace RanobeNet.Controllers
{
    public partial class NovelsController
	{
		[HttpGet("{id}/episodes/{episodeId}")]
		public async Task<ActionResult<EpisodeDtoForPublicParsed>> GetEpisode(long id, long episodeId)
		{
			return await novelRepository.GetEpisode(id, episodeId);
		}

		[Authorize]
		[HttpGet("{id}/episodes/{episodeId}/me")]
		public async Task<ActionResult<EpisodeDtoForMe>> GetEpisodeForMe(long id, long episodeId)
		{
			var firebaseUid = this.HttpContext.GetFirebaseUid();
			return await novelRepository.GetEpisodeForMe(id, episodeId, firebaseUid);
		}

		[Authorize]
		[HttpPost("{id}/episodes")]
		public async Task<ActionResult<EpisodeDtoForMe>> CreateEpisode(long id, EpisodeDtoForSave episode)
		{
			var firebaseUid = this.HttpContext.GetFirebaseUid();
			return await novelRepository.CreateEpisode(id, firebaseUid, episode);
		}

		[Authorize]
		[HttpPut("{id}/episodes/{episodeId}")]
		public async Task<ActionResult<EpisodeDtoForMe>> UpdateEpisode(long id, long episodeId, EpisodeDtoForSave episode)
		{
			var firebaseUid = this.HttpContext.GetFirebaseUid();
			return await novelRepository.UpdateEpisode(id, episodeId, firebaseUid, episode);
		}

		[Authorize]
		[HttpDelete("{id}/episodes/{episodeId}")]
		public async Task<ActionResult> DeleteEpisode(long id, long episodeId)
		{
			var firebaseUid = this.HttpContext.GetFirebaseUid();
			await novelRepository.DeleteEpisode(id, episodeId, firebaseUid);
			return Ok();
		}

	}
}
