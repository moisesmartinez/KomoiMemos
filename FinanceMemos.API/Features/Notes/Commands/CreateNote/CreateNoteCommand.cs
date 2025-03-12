using MediatR;

namespace FinanceMemos.API.Features.Notes.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<CreateNoteResponse>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CreateNoteResponse
    {
        public int NoteId { get; set; }
        public string Message { get; set; }
    }
}
