using AutoMapper;
using EmailSender.Core.DTOs.Account;
using EmailSender.Data.Entities.Users;

namespace EmailSender.Core.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, ProfileDto>();
        }
    }
}
