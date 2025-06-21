using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTicket.Application.Abstracts;
using WebTicket.Application.Contracts;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Requests;





namespace WebTicket.API.Controller
{
    [Route("Unitic/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _emailService;

        public AuthController(IAccountService service, UserManager<User> userManager, IMailService emailService)
        {
            // Constructor logic if needed
            _accountService = service;
            _userManager = userManager;
            _emailService = emailService;
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
                return Unauthorized("Failed");
            }

            string accessToken = await _accountService.LoginWithGoogleAsync(authenticateResult.Principal);

            return Ok(new LoginResponse
            {
                Message = "Login successful.",
                // Trả về token vào response body để tránh độ trễ lần đầu tạo cookie
                Token = accessToken
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return BadRequest("User not found!");

            var emailRequest = await _accountService.CreateOtp(request.Email);

            await _emailService.SendEmailAsync(emailRequest);

            return Ok("OTP sent to email");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpRequest otpRequest)
        {
           (bool status, string token) = await _accountService.VerifyOtp(otpRequest.Email, otpRequest.Otp);
            if (status)
            {
                return Ok(new
                {
                    Message = "OTP verified successfully",
                    Token = token
                });
            }
            return BadRequest("OTP expired or not found");
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> resetPassword([FromBody]ResetPasswordRequest request)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("User not found");
            }
            await _accountService.ChangePassword(user, request.Password);
            Response.Cookies.Delete("RESET_TOKEN"); // Xóa cookie reset password
            return Ok("Password reset successfully");
        }



    }

}

