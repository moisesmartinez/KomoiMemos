using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;

namespace FinanceMemos.API.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly KomoiMemosDbContext _context;

        public NoteRepository(KomoiMemosDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task<Note> GetByIdAsync(int id)
        {
            return await _context.Notes.FindAsync(id);
        }
    }
}
