using MediatR;

namespace FinanceMemos.API.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQuery : IRequest<GetEventByIdResponse>
    {
        public int EventId { get; set; }
    }

    public class GetEventByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
