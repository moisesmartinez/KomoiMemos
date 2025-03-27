using FinanceMemos.API.Data;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, GetExpenseByIdResponse>
    {
        private readonly IExpenseRepository _expenseRepository;

        public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<GetExpenseByIdResponse> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId);

            if (expense == null)
            {
                throw new System.Collections.Generic.KeyNotFoundException("Expense not found.");
            }

            return new GetExpenseByIdResponse
            {
                Id = expense.Id,
                EventId = expense.EventId,
                Amount = expense.Amount,
                Category = expense.Category,
                Date = expense.Date,
                Description = expense.Description,
                UserId = expense.UserId,
                CreatedAt = expense.CreatedAt
            };
        }
    }
}
