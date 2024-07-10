using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.IdentityModel.Tokens;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Helper;
using MyApiNetCore8.Repositories;

namespace MyApiNetCore8.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _accountService.SignUpAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("token")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var (accessToken, refreshToken) = await _accountService.SignInAsync(model);
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse<object>(1000, "Success", new { accessToken = accessToken, refreshToken = refreshToken }));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {

            var newRefreshToken = await _accountService.RefreshTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(newRefreshToken.Item2))
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse<object>(1000, "Success", new { accessToken = newRefreshToken.Item1, refreshToken = newRefreshToken.Item2 }));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> GetMyInfo()
        {
            var account = await _accountService.GetMyInfoAsync();
            if (account == null)
            {
                return NotFound();
            }
            return Ok(new ApiResponse<AccountResponse>(1000, "Success", account));
        }

        [HttpGet("customers")]
        [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
        public async Task<ActionResult<ApiResponse<List<AccountResponse>>>> GetAllCustomer()
        {
            var customers = await _accountService.GetAllCustomersAsync();
            return Ok(new ApiResponse<List<AccountResponse>>(1000, "Success", customers));
        }

        [HttpGet("admins")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<ActionResult<ApiResponse<List<AccountResponse>>>> GetAllSystemUsers()
        {
            var customers = await _accountService.GetAllSystemUsersAsync();
            return Ok(new ApiResponse<List<AccountResponse>>(1000, "Success", customers));
        }
    }
}
