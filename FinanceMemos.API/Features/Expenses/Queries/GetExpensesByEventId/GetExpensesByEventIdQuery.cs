using MediatR;

namespace FinanceMemos.API.Features.Expenses.Queries.GetExpensesByEventId
{
    public class GetExpensesByEventIdQuery : IRequest<List<GetExpensesByEventIdResponse>>
    {
        public int EventId { get; set; }
    }

    public class GetExpensesByEventIdResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
