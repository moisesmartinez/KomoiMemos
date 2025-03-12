using FinanceMemos.API.Data;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;

namespace FinanceMemos.API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly KomoiMemosDbContext _context;

        public ImageRepository(KomoiMemosDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }
    }
}
