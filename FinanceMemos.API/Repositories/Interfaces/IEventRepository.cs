using FinanceMemos.API.Models;

namespace FinanceMemos.API.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task AddAsync(Event myEvent);
        Task<Event> GetByIdAsync(int id);
        Task<List<Event>> GetByUserIdAsync(int id);
        Task<bool> IsEventFound(int eventId);
    }
}
