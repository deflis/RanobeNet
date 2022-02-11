#nullable disable
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Models.Dto;
using RanobeNet.Repositories;
using RanobeNet.Utils;
using System.ComponentModel;

namespace RanobeNet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly INovelRepository novelRepository;
        private readonly FirebaseAuth firebaseAuth;

        public UsersController(IUserRepository userRepository, INovelRepository novelRepository, FirebaseAuth firebaseAuth)
        {
            this.userRepository = userRepository;
            this.novelRepository = novelRepository;
            this.firebaseAuth = firebaseAuth;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserDtoForPublicListing>>> GetUsers([FromQuery] GetUsersQueryParams queryParam)
        {
            var builder = QueryBuilder<User>.create(queryParam.page, queryParam.size).SetDescending(queryParam.descending);
            switch (queryParam.order)
            {
                case UserField.id:
                    builder.SetKeySelector(x => x.Id);
                    break;
                case UserField.name:
                    builder.SetKeySelector(x => x.Name);
                    break;
                case UserField.created:
                    builder.SetKeySelector(x => x.CreatedAt);
                    break;
                case UserField.modified:
                    builder.SetKeySelector(x => x.UpdatedAt);
                    break;
            }
            return await userRepository.GetUsers(builder.build());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDtoForPublic>> GetUser(long id)
        {
            var user = await userRepository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDtoForMe>> GetUserMe()
        {
            var firebaseUid = this.HttpContext.User.GetFirebaseUid();
            var user = await userRepository.GetUserMe(firebaseUid);

            if (user == null)
            {
                return NotFound();

            }
            return user;
        }

        [Authorize]
        [HttpPost("me")]
        public async Task<ActionResult<UserDtoForMe>> GetOrUpdateUserMe()
        {
            var firebaseUid = this.HttpContext.User.GetFirebaseUid();
            var updatedUser = await userRepository.GetOrAddUserMe(firebaseUid, async () => (await firebaseAuth.GetUserAsync(firebaseUid)).DisplayName);

            if (updatedUser == null)
            {
                return NotFound();
            }
            return updatedUser;
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult<UserDtoForMe>> UpdateUserMe(UserDtoForSave user)
        {
            var firebaseUid = this.HttpContext.User.GetFirebaseUid();
            var updatedUser = await userRepository.UpdateUserMe(firebaseUid, user);

            if (updatedUser == null)
            {
                return NotFound();

            }
            return updatedUser;
        }
    }
}
