using FinanceMemos.API.Models;

namespace FinanceMemos.API.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense);
        Task<Expense> GetByIdAsync(int id);
    }
}
