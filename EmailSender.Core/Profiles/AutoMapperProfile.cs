using AutoMapper;
using EmailSender.Core.DTOs.Account;
using EmailSender.Data.Entities.Users;

namespace EmailSender.Core.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Account

            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, ProfileDto>();
            CreateMap<UpdateProfileDto, AppUser>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, sourceMember) =>
                    sourceMember != null)); ;

            #endregion
        }
    }
}
