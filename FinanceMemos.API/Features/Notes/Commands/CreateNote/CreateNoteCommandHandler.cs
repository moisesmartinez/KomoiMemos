using FinanceMemos.API.CustomExceptions;
using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using FinanceMemos.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Features.Notes.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteResponse>
    {
        private readonly KomoiMemosDbContext _context;
        private readonly INoteRepository _noteRepository;
        private readonly IImageRepository _imageRepository;
        private readonly S3Service _s3Service;

        public CreateNoteCommandHandler(KomoiMemosDbContext context,
            INoteRepository noteRepository,
            IImageRepository imageRepository,
            S3Service s3Service)
        {
            _context = context;
            _noteRepository = noteRepository;
            _s3Service = s3Service;
            _imageRepository = imageRepository;
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

            // Upload image to S3 if provided
            string imageUrl = string.Empty;
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                using (var stream = request.ImageFile.OpenReadStream())
                {
                    var fileName = $"{Guid.NewGuid()}_{request.ImageFile.FileName}";
                    imageUrl = await _s3Service.UploadFileAsync(stream, fileName);
                }
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new InputValidationException("Error uploading image. Try again.");
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


            var image = new Image
            {
                ImageUrl = imageUrl,
                NoteId = note.Id,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _imageRepository.AddAsync(image);

            return new CreateNoteResponse
            {
                NoteId = note.Id,
                Message = "Note created successfully."
            };
        }
    }
}
