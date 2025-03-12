using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Repositories;

public class EventRepository : IEventRepository
{
    private readonly KomoiMemosDbContext _context;

    public EventRepository(KomoiMemosDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event myEvent)
    {
        _context.Events.Add(myEvent);
        await _context.SaveChangesAsync();
    }

    public async Task<Event> GetByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<List<Event>> GetByUserIdAsync(int userId)
    {
        return await _context
            .Events
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsEventFound(int eventId)
    {
        return await _context
            .Events
            .AnyAsync(x => x.Id == eventId);
    }
}
