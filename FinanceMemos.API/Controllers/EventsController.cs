using FinanceMemos.API.Data;
using FinanceMemos.API.Features.Events.Commands.CreateEvent;
using FinanceMemos.API.Features.Events.Queries.GetEventById;
using FinanceMemos.API.Features.Events.Queries.GetEventsByUserId;
using FinanceMemos.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceMemos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var query = new GetEventByIdQuery { EventId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("user-events")]
        public async Task<IActionResult> GetEventsByUserId()
        {
            // Extract UserId from the token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User ID not found in the token." });
            }

            // Fetch events for the user
            var query = new GetEventsByUserIdQuery { UserId = int.Parse(userId) };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}