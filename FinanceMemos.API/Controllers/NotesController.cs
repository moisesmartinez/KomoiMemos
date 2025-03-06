using FinanceMemos.API.Features.Notes.Commands.CreateNote;
using FinanceMemos.API.Features.Notes.Queries.GetNoteById;
using FinanceMemos.API.Features.Notes.Queries.GetNotesByEventId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceMemos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;
    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("notes/{id}")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var query = new GetNoteByIdQuery { NoteId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("notes")]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteCommand command)
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
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetNotesByEventId(int eventId)
    {
        var query = new GetNotesByEventIdQuery { EventId = eventId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
