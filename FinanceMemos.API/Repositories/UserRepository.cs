using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly KomoiMemosDbContext _context;

    public UserRepository(KomoiMemosDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserFound(int userId)
    {
        return await _context.Users.AnyAsync(x => x.Id == userId);
    }
}
