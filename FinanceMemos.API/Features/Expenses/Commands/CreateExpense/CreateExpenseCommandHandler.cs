using FinanceMemos.API.CustomExceptions;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;

namespace FinanceMemos.API.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, CreateExpenseResponse>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;

    public CreateExpenseCommandHandler(IExpenseRepository expenseRepository,
        IUserRepository userRepository,
        IEventRepository eventRepository)
    {
        _expenseRepository = expenseRepository;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
    }

    public async Task<CreateExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        if(await _userRepository.IsUserFound(request.UserId) == false 
            || await _eventRepository.IsEventFound(request.EventId) == false)
        {
            throw new InputValidationException("Error inserting Expense. Try again.");
        }
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
