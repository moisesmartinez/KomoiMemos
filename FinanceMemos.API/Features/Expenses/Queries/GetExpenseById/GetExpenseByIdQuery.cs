using MediatR;

namespace FinanceMemos.API.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdQuery : IRequest<GetExpenseByIdResponse>
    {
        public int ExpenseId { get; set; }
    }

    public class GetExpenseByIdResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
