using FinanceMemos.API.Data;
using FinanceMemos.API.Features.Events.Commands.CreateEvent;
using FinanceMemos.API.Features.Events.Queries.GetEventById;
using FinanceMemos.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}