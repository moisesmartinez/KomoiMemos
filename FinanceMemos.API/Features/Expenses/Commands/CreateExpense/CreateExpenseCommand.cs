using MediatR;

namespace FinanceMemos.API.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommand : IRequest<CreateExpenseResponse>
    {
        public int EventId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }

    public class CreateExpenseResponse
    {
        public int ExpenseId { get; set; }
        public string Message { get; set; }
    }
}
