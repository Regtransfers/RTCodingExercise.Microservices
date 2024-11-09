using Catalog.Domain.Entities;

namespace WebMVC.Infrastructure.Data.Repositories
{
    public sealed class PlateRepository : IPlateRepository
    {
        private readonly ApplicationDbContext _context;

        public PlateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plate>> GetAllAsync()
        {
            return await _context.Plates
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Plate>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Plates
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}