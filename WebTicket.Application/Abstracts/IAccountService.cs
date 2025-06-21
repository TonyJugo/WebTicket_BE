using System.Security.Claims;
using WebTicket.Application.Contracts;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Requests;

namespace WebTicket.Application.Abstracts;

public interface IAccountService
{
    Task RegisterAsync(RegisterRequest registerRequest);
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task<string> LoginWithGoogleAsync(ClaimsPrincipal? principal);
    Task<SendEmailRequest> CreateOtp(string email);
    Task<(bool, string)> VerifyOtp(string email, string otp);
    Task ChangePassword(string userId, string newPassword);
}