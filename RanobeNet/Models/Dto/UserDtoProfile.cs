using AutoMapper;
using RanobeNet.Models.Data;

namespace RanobeNet.Models.Dto
{
    public class UserDtoProfile : Profile
    {
        public UserDtoProfile()
        {
            CreateMap<User, UserDtoForPublicListing>();
            CreateMap<User, UserDtoForMe>();
            CreateMap<UserDtoForSave, User>();
        }
    }
}
