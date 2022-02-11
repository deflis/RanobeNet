using RanobeNet.Data;
using RanobeNet.Models.Data;
using RanobeNet.Models.Dto;

namespace RanobeNet.Repositories
{
    public interface IUserRepository
    {
        Task<UserDtoForPublic?> GetUser(long id);
        Task<PagedList<UserDtoForPublicListing>> GetUsers(Query<User> query);
        Task<User?> ResolveUser(string firebaseUid);
        Task<UserDtoForMe> GetOrAddUserMe(string firebaseUid, Func<Task<string>> getName);
        Task<UserDtoForMe?> GetUserMe(string firebaseUid);
        Task<UserDtoForMe> UpdateUserMe(string firebaseUid, UserDtoForSave user);
    }
}