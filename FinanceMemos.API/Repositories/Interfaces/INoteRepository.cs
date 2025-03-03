using FinanceMemos.API.Models;

namespace FinanceMemos.API.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task AddAsync(Note note);
        Task<Note> GetByIdAsync(int id);
    }
}
