
using System.Security.Claims;
using EmailSender.Core.DTOs.Account;
using EmailSender.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace EmailSender.Core.Interfaces
{
    public interface IAccountService
    {
        Task<string> GenerateJwt(AppUser appUser);
        Task<IdentityResult> CreateUser(RegisterDto registerDto);
        Task<ProfileDto> GetCurrentUserProfile(ClaimsPrincipal userClaims);
    }
}
