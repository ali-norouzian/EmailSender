using System.IdentityModel.Tokens.Jwt;
using EmailSender.Core.Interfaces;
using EmailSender.Data.Entities.Users;
using System.Security.Claims;
using System.Text;
using EmailSender.Core.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountService(IConfiguration config, UserManager<AppUser> userManager, IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<string> GenerateJwt(AppUser appUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Name, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.NameId, appUser.Id)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireTimePerMin"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerDto)
        {
            var user = _mapper.Map<AppUser>(registerDto);
            user.UserName = user.Email.Split('@')[0];

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            return result;
        }

        public async Task<IdentityResult> UpdateUser(UpdateProfileDto dto, ClaimsPrincipal userClaims)
        {
            var authUserId = GetCurrentUserId(userClaims);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == authUserId);
            user = _mapper.Map(dto, user);
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public string GetCurrentUserId(ClaimsPrincipal userClaims)
        {
            return userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<ProfileDto> GetCurrentUserProfile(ClaimsPrincipal userClaims)
        {
            var authUserId = GetCurrentUserId(userClaims);
            var user = await _userManager.FindByIdAsync(authUserId);
            var profileDto = _mapper.Map<ProfileDto>(user);

            return profileDto;
        }
    }
}
