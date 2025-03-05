using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;

namespace FinanceMemos.API.Features.Events.Queries.GetEventsByUserId
{
    public class GetEventsByUserIdQueryHandler : IRequestHandler<GetEventsByUserIdQuery, List<Event>>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventsByUserIdQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<Event>> Handle(GetEventsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetByUserIdAsync(request.UserId);
        }
    }
}
