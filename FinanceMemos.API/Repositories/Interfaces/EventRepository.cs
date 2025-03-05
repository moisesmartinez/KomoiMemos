using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Repositories.Interfaces
{
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

        [Authorize]
        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        [Authorize]
        public async Task<List<Event>> GetByUserIdAsync(int userId)
        {
            return await _context
                .Events
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
