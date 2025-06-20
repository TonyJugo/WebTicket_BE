
using Microsoft.EntityFrameworkCore;
using WebTicket.Application.Abstracts;
using WebTicket.Domain.Entities;

namespace WebTicket.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<string> GetLastId()
    {
        string id = await _applicationDbContext.Users
            .OrderByDescending(u => u.Id)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();
        return id;
    }

    public async Task<string> GetUniversityIdByNameAsync(string universityName)
    {
        var university = await _applicationDbContext.Universities
            .FirstOrDefaultAsync(u => u.Name == universityName);
        if(university != null)
        {
            return university.Id;
        }
        return null;
    }

    public async Task<User> GetUserById(string userId)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _applicationDbContext.Users.Update(user);
        await _applicationDbContext.SaveChangesAsync();
    }




}