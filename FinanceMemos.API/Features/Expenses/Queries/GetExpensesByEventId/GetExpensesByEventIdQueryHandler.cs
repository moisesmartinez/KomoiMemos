using FinanceMemos.API.Data;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Features.Expenses.Queries.GetExpensesByEventId
{
    public class GetExpensesByEventIdQueryHandler : IRequestHandler<GetExpensesByEventIdQuery, List<GetExpensesByEventIdResponse>>
    {
        private readonly IExpenseRepository _expenseRepository;

        public GetExpensesByEventIdQueryHandler(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<List<GetExpensesByEventIdResponse>> Handle(GetExpensesByEventIdQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _expenseRepository.GetByEventIdAsync(request.EventId);

            var response = expenses.Select(e => new GetExpensesByEventIdResponse
            {
                Id = e.Id,
                Amount = e.Amount,
                Category = e.Category,
                Date = e.Date,
                Description = e.Description,
                CreatedAt = e.CreatedAt
            }).ToList();

            return response;
        }
    }
}
