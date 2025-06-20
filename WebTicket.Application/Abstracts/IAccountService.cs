using WebTicket.Domain.Requests;

namespace WebTicket.Application.Abstracts;

public interface IAccountService
{
    Task RegisterAsync(RegisterRequest registerRequest);
    Task<string> LoginAsync(LoginRequest loginRequest);

}