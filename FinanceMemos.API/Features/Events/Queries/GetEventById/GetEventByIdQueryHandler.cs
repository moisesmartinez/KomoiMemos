using FinanceMemos.API.Repositories.Interfaces;
using MediatR;

namespace FinanceMemos.API.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdResponse>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventByIdQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<GetEventByIdResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var myEvent = await _eventRepository.GetByIdAsync(request.EventId);
            if (myEvent == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }

            return new GetEventByIdResponse
            {
                Id = myEvent.Id,
                Name = myEvent.Name,
                Description = myEvent.Description,
                UserId = myEvent.UserId,
                CreatedAt = myEvent.CreatedAt
            };
        }
    }
}
