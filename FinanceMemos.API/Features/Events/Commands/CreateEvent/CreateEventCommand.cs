using MediatR;

namespace FinanceMemos.API.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommand : IRequest<CreateEventResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; } // Added UserId to associate the event with a user
    }

    public class CreateEventResponse
    {
        public int EventId { get; set; }
        public string Message { get; set; }
    }
}
