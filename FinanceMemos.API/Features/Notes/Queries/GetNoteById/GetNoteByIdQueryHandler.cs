using FinanceMemos.API.Data;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Features.Notes.Queries.GetNoteById
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, GetNoteByIdResponse>
    {
        private readonly INoteRepository _noteRepository;

        public GetNoteByIdQueryHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<GetNoteByIdResponse> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetByIdAsync(request.NoteId);

            if (note == null)
            {
                throw new KeyNotFoundException("Note not found.");
            }

            var response = new GetNoteByIdResponse
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                Type = note.Type,
                CreatedAt = note.CreatedAt
            };

            return response;
        }
    }

}
