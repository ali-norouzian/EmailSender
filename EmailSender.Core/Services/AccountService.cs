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
using EmailSender.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace EmailSender.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AccountService(IConfiguration config, UserManager<AppUser> userManager, IMapper mapper, DataContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
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

        public async Task<int> GetCountOfUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<bool> EmailSendingGroupIsPending(int groupId)
        {
            return (await _context.EmailSendingGroup.FindAsync(groupId)).IsPending;
        }

        private async Task TrySendEmail(object inpUser, int groupId)
        {
            var user = (dynamic)inpUser;

            // Send email for sample
            await Task.Delay(1000);

            Random random = new Random();
            var sendIsSucceed = random.Next(2) == 0;

            if (!sendIsSucceed)
            {
                TrySendEmail(user, groupId);
            }
            else
            {
                await using (var scope = _serviceScopeFactory.CreateAsyncScope())
                {
                    var services = scope.ServiceProvider;

                    var dbcontext = services.GetRequiredService<DataContext>();


                    await dbcontext.EmailSendingStatus.AddAsync(new EmailSendingStatus()
                    {
                        AppUserId = user.Id,
                        GroupId = groupId,
                        Successful = true,
                    });

                    var isSuccess = await dbcontext.SaveChangesAsync() > 0;
                    //if (!isSuccess)
                    //{
                    //}
                }
            }
        }

        public async Task SendEmailTo(int groupId, DateTime? startJoinedDate = null, DateTime? endJoinedDate = null)
        {
            var group = await _context.EmailSendingGroup.FindAsync(groupId);
            group.IsPending = true;
            await _context.SaveChangesAsync();

            // ToDo: Check that have not recently unsuccessful email sending
            var users = await _userManager.Users.Select(u => new
            {
                Id = u.Id,
                Email = u.Email,
            }).ToListAsync();

            foreach (var user in users)
            {
                await TrySendEmail(user, groupId);
            }


            group.IsPending = false;
            await _context.SaveChangesAsync();
        }

        // Return sent, remain, total
        public async Task<(int, int, int)> GetGroupEmailSendingStatus(int groupId)
        {
            var userCount = await GetCountOfUsers();

            var sentCount = await _context.EmailSendingStatus
                .Where(e => e.GroupId == groupId)
                .CountAsync();

            var remainCount = userCount - sentCount;
            var total = remainCount + sentCount;

            return (sentCount, remainCount, total);
        }
    }
}
