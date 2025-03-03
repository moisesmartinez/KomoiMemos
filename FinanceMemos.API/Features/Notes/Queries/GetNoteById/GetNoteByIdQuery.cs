using MediatR;

namespace FinanceMemos.API.Features.Notes.Queries.GetNoteById
{
    public class GetNoteByIdQuery : IRequest<GetNoteByIdResponse>
    {
        public int NoteId { get; set; }
    }

    public class GetNoteByIdResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
