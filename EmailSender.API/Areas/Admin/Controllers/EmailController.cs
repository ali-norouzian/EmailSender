using AutoMapper;
using EmailSender.Core.Interfaces;
using EmailSender.Data.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.API.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("api/admin/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public EmailController(UserManager<AppUser> userManager, IAccountService accountService, IMapper mapper)
        {
            _userManager = userManager;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountOfEmails()
        {
            var userCount = await _accountService.GetCountOfUsers();

            return Ok(new { UserCount = userCount });
        }

        [HttpGet("send/{groupId}")]
        public async Task<IActionResult> SendTo(int groupId, DateTime? startJoinedDate, DateTime? endJoinedDate)
        {
            var isPending = await _accountService.EmailSendingGroupIsPending(groupId);
            if (isPending)
                return Conflict(new { message = "Sending email to this group is already in program. please wait to finish it." });

            _accountService.SendEmailTo(1);

            return Ok();
        }

        [HttpGet("send/{groupId}/status")]
        public async Task<IActionResult> SendTo(int groupId)
        {
            var (sent, remain, total) = await _accountService.GetGroupEmailSendingStatus(groupId);

            return Ok(new
            {
                sent,
                remain,
                total,
            });
        }

    }
}
