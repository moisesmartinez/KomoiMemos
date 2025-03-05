using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly KomoiMemosDbContext _context;

        public ExpenseRepository(KomoiMemosDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<Expense> GetByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task<List<Expense>> GetByEventIdAsync(int eventId)
        {
            return await _context.Expenses
                .Where(e => e.EventId == eventId)
                .ToListAsync();
        }
    }
}
