using FinanceMemos.API.Models;
using MediatR;

namespace FinanceMemos.API.Features.Events.Queries.GetEventsByUserId
{
    public class GetEventsByUserIdQuery : IRequest<List<Event>>
    {
        public int UserId { get; set; }
    }
}
