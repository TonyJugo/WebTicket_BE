using WebTicket.Domain.Entities;

namespace WebTicket.Application.Abstracts;

public interface IAuthTokenProcessor
{
    (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles);
    public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token,
    DateTime expiration);
}