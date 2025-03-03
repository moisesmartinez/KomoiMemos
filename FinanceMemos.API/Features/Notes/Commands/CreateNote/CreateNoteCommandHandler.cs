using FinanceMemos.API.CustomExceptions;
using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Features.Notes.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteResponse>
    {
        private readonly KomoiMemosDbContext _context;
        private readonly INoteRepository _noteRepository;

        public CreateNoteCommandHandler(KomoiMemosDbContext context,
            INoteRepository noteRepository)
        {
            _context = context;
            _noteRepository = noteRepository;
        }

        public async Task<CreateNoteResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            // Validate EventId
            var eventExists = await _context.Events.AnyAsync(e => e.Id == request.EventId, cancellationToken);
            if (!eventExists)
            {
                throw new InputValidationException("EventId does not exist.");
            }

            // Validate UserId
            var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
            if (!userExists)
            {
                throw new InputValidationException("UserId does not exist.");
            }

            var note = new Note
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                EventId = request.EventId,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _noteRepository.AddAsync(note);

            return new CreateNoteResponse
            {
                NoteId = note.Id,
                Message = "Note created successfully."
            };
        }
    }
}
