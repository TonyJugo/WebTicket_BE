using WebTicket.Domain.Entities;

namespace WebTicket.Application.Abstracts;

public interface IUserRepository
{
    Task<string> GetUniversityIdByNameAsync(string universityName);
    Task<User> GetUserById(string userId);
    Task<string> GetLastId();
    Task UpdateAsync(User user);
}