﻿
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
        Task<IdentityResult> UpdateUser(UpdateProfileDto dto, ClaimsPrincipal userClaims);
        string GetCurrentUserId(ClaimsPrincipal userClaims);
        Task<ProfileDto> GetCurrentUserProfile(ClaimsPrincipal userClaims);
        Task<int> GetCountOfUsers();
        Task<bool> EmailSendingGroupIsPending(int groupId);
        Task SendEmailTo(int groupId, DateTime? startJoinedDate = null, DateTime? endJoinedDate = null);
        Task<(int, int, int)> GetGroupEmailSendingStatus(int groupId);
    }

}
