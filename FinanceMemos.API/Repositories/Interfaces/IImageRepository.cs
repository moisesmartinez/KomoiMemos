using FinanceMemos.API.Models;

namespace FinanceMemos.API.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task AddAsync(Image image);
    }
}
