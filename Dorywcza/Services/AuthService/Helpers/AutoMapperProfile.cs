using AutoMapper;
using Dorywcza.Models.Auth;

namespace Dorywcza.Services.AuthService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserRegister, User>();
            CreateMap<UserUpdate, User>();
        }
    }
}
