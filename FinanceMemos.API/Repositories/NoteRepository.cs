using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Note>> GetByEventIdAsync(int eventId)
        {
            //Return the notes with images by EventId
            return await _context.Notes
                .Include(n => n.Images)
                .Where(n => n.EventId == eventId)
                .ToListAsync();

            //return await _context.Notes
            //    .Where(n => n.EventId == eventId)
            //    .ToListAsync();
        }
    }
}
