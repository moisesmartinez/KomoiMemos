using FinanceMemos.API.Data;
using FinanceMemos.API.Repositories.Interfaces;
using MediatR;

namespace FinanceMemos.API.Features.Notes.Queries.GetNotesByEventId
{
    public class GetNotesByEventIdQueryHandler : IRequestHandler<GetNotesByEventIdQuery, List<GetNotesByEventIdResponse>>
    {
        private readonly INoteRepository _noteRepository;

        public GetNotesByEventIdQueryHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<List<GetNotesByEventIdResponse>> Handle(GetNotesByEventIdQuery request, CancellationToken cancellationToken)
        {
            var notes = await _noteRepository.GetByEventIdAsync(request.EventId);

            var response = notes
                .Select(n => new GetNotesByEventIdResponse
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    Type = n.Type,
                    ImageUrl = n.Images.FirstOrDefault()?.ImageUrl,
                    CreatedAt = n.CreatedAt
                })
                .ToList();

            return response;
        }
    }
}
