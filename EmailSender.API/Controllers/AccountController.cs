using AutoMapper;
using EmailSender.Core.DTOs.Account;
using EmailSender.Core.Interfaces;
using EmailSender.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EmailSender.API.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, IAccountService accountService, IMapper mapper)
        {
            _userManager = userManager;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _accountService.CreateUser(registerDto);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return NotFound();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (result == false) return Unauthorized();

            return Ok(new { Token = await _accountService.GenerateJwt(user) });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ProfileDetail()
        {
            var profileDto = await _accountService.GetCurrentUserProfile(User);

            return Ok(profileDto);
        }


    }
}
