using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using System.Security.Claims;

namespace FinanceMemos.API.GraphQL.Queries;

public class EventQuery
{
    public IQueryable<Event> GetUserEvents(
        [Service] KomoiMemosDbContext dbContext,
        ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return dbContext.Events.Where(e => e.UserId == userId);
    }
}
