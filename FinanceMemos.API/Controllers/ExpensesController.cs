using FinanceMemos.API.Features.Expenses.Commands.CreateExpense;
using FinanceMemos.API.Features.Expenses.Queries.GetExpenseById;
using FinanceMemos.API.Features.Expenses.Queries.GetExpensesByEventId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        // Extract UserId from the token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { Message = "User ID not found in the token." });
        }

        command.UserId = int.Parse(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("expenses/{id}")]
    public async Task<IActionResult> GetExpenseById(int id)
    {
        var query = new GetExpenseByIdQuery { ExpenseId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetExpensesByEventId(int eventId)
    {
        var query = new GetExpensesByEventIdQuery { EventId = eventId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

