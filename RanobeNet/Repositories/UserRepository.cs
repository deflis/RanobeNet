using AutoMapper;
using RanobeNet.Data;
using RanobeNet.Models.Dto;
using Microsoft.EntityFrameworkCore;
using RanobeNet.Models.Data;
using RanobeNet.Utils;

namespace RanobeNet.Repositories
{
    public class UserRepository : IUserRepository
    {
        private RanobeNetContext context;
        private IMapper mapper;

        public UserRepository(RanobeNetContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<User?> ResolveUser(string firebaseUid)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.FirebaseUid == firebaseUid);
        }

        public async Task<PagedList<UserDtoForPublicListing>> GetUsers(Query<User> query, bool includesRomUser = false)
        {
            var users = includesRomUser ? context.Users : context.Users.Where(x => x.Novels.Count > 0);
            return await users.ToPagedListAsync<User, UserDtoForPublicListing>(query, mapper);
        }

        public async Task<UserDtoForPublic?> GetUser(long id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return null;
            return new UserDtoForPublic
            {
                Id = user.Id,
                Name = user.Name,
                Novels = user.Novels.OrderByDescending(x => x.UpdatedAt).Select(x => mapper.Map<NovelDtoForPublicListing>(x)).ToList()
            };
        }

        public async Task<UserDtoForMe?> GetUserMe(string firebaseUid)
        {
            return mapper.Map<UserDtoForMe>(await context.Users.SingleOrDefaultAsync(x => x.FirebaseUid == firebaseUid));
        }

        public async Task<UserDtoForMe> GetOrAddUserMe(string firebaseUid, Func<Task<string>> getName)
        {
            var exists = await context.Users.AnyAsync(x => x.FirebaseUid == firebaseUid);
            var user = await context.Users.SingleOrDefaultAsync(x => x.FirebaseUid == firebaseUid);
            if (user == null)
            {
                user = new User();
                user.FirebaseUid = firebaseUid;
                user.Name = await getName();
                await context.AddAsync(user);
                await context.SaveChangesAsync();
            }
            return mapper.Map<UserDtoForMe>(user);
        }
        public async Task<UserDtoForMe> UpdateUserMe(string firebaseUid, UserDtoForSave user)
        {
            var rawUser = await context.Users.SingleAsync(x => x.FirebaseUid == firebaseUid);
            rawUser.Name = user.Name;
            await context.SaveChangesAsync();
            return mapper.Map<UserDtoForMe>(rawUser);
        }
    }
}
