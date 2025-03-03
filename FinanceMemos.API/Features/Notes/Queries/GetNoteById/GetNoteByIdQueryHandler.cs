using FinanceMemos.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Features.Notes.Queries.GetNoteById
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, GetNoteByIdResponse>
    {
        private readonly KomoiMemosDbContext _context;

        public GetNoteByIdQueryHandler(KomoiMemosDbContext context)
        {
            _context = context;
        }

        public async Task<GetNoteByIdResponse> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var note = await _context.Notes
                .Where(n => n.Id == request.NoteId)
                .Select(n => new GetNoteByIdResponse
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    Type = n.Type,
                    EventId = n.EventId,
                    UserId = n.UserId,
                    CreatedAt = n.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (note == null)
            {
                throw new KeyNotFoundException("Note not found.");
            }

            return note;
        }
    }

}
