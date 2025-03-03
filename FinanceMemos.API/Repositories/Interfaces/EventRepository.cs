using FinanceMemos.API.Data;
using FinanceMemos.API.Models;

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

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }
    }
}
