using FinanceMemos.API.Features.Notes.Commands.CreateNote;
using FinanceMemos.API.Features.Notes.Queries.GetNoteById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("notes")]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
