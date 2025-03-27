using FinanceMemos.API.Data;
using FinanceMemos.API.GraphQL.Inputs;
using FinanceMemos.API.GraphQL.Validators;
using FinanceMemos.API.Models;
using System.Security.Claims;

namespace FinanceMemos.API.GraphQL.Mutation
{
    public class EventMutation
    {
        public async Task<Event> CreateEvent(
            CreateEventInput input,
            [Service] KomoiMemosDbContext dbContext,
            ClaimsPrincipal user)
        {
            var validator = new CreateEventValidator();
            var validationResult = validator.Validate(input);
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int userId;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out userId))
            {
                userId = 1; // Hardcoded for testing purposes
            }

            var newEvent = new Event
            {
                Name = input.Name,
                Description = input.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Events.Add(newEvent);
            await dbContext.SaveChangesAsync();
            return newEvent;
        }
    }
}
