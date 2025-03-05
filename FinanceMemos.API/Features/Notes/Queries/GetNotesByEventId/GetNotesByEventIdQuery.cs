using MediatR;

namespace FinanceMemos.API.Features.Notes.Queries.GetNotesByEventId
{
    public class GetNotesByEventIdQuery : IRequest<List<GetNotesByEventIdResponse>>
    {
        public int EventId { get; set; }
    }

    public class GetNotesByEventIdResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
