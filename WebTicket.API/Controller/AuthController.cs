using Azure.Core;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebTicket.Application.Abstracts;
using WebTicket.Domain.Requests;




namespace WebTicket.API.Controller
{
    [Route("Unitic/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;


        public AuthController(IAccountService service)
        {
            // Constructor logic if needed
            _accountService = service;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("Invalid registration request.");
            }
            await _accountService.RegisterAsync(registerRequest);
            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request.");
            }
            string accessToken = await _accountService.LoginAsync(loginRequest);

            return Ok(new LoginResponse
            {
                Message = "Login successful.",
                // Trả về token vào response body để tránh độ trễ lần đầu tạo cookie
                Token = accessToken
            });
        }

        [HttpPost("logout")]
        [Authorize(Roles = "User")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("ACCESS_TOKEN"); // Xóa cookie đăng nhập

            return Ok("Log out successful");
        }

        [HttpGet("google-request")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Auth", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleOpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleOpenIdConnectDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized("HAHA");
            }

            string accessToken = await _accountService.LoginWithGoogleAsync(authenticateResult.Principal);

            return Ok(new LoginResponse
            {
                Message = "Login successful.",
                // Trả về token vào response body để tránh độ trễ lần đầu tạo cookie
                Token = accessToken
            });

        }

    }

}

