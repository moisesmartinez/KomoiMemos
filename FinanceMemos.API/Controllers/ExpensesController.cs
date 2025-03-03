using FinanceMemos.API.Features.Expenses.Commands.CreateExpense;
using FinanceMemos.API.Features.Expenses.Queries.GetExpenseById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceMemos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("expenses")]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("expenses/{id}")]
    public async Task<IActionResult> GetExpenseById(int id)
    {
        var query = new GetExpenseByIdQuery { ExpenseId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

