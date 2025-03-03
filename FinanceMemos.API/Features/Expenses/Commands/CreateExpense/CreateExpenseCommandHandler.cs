using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;

namespace FinanceMemos.API.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, CreateExpenseResponse>
    {
        private readonly IExpenseRepository _expenseRepository;

        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<CreateExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expense
            {
                EventId = request.EventId,
                Amount = request.Amount,
                Category = request.Category,
                Date = request.Date,
                Description = request.Description,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _expenseRepository.AddAsync(expense);

            return new CreateExpenseResponse
            {
                ExpenseId = expense.Id,
                Message = "Expense created successfully."
            };
        }
    }
}
