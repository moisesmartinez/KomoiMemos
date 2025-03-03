using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;

namespace FinanceMemos.API.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
    {
        private readonly IEventRepository _eventRepository;

        public CreateEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var myEvent = new Event
            {
                Name = request.Name,
                Description = request.Description,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.AddAsync(myEvent);

            return new CreateEventResponse
            {
                EventId = myEvent.Id,
                Message = "Event created successfully."
            };
        }
    }
}
