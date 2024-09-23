using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.Interfaces.Services;

namespace RedSocial.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AutheticationAsync(request));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterBasicUserAsync(request, origin));
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> RegisterAsync([FromQuery] string userId, [FromQuery] string Token)
        {
            return Ok(await _accountService.ConfirmEmailAsync(userId, Token));
        }


        [HttpPost("forgor-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetpasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsyncs(request));
        }
    }
}
