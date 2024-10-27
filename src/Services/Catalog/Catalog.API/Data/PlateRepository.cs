using Catalog.API.Requests;
using Catalog.DTO;

namespace Catalog.API.Data
{
    public class PlateRepository : IPlateRepository
    {
        const decimal SalesPriceMultiplier = 1.2m;
        const int PageSize = 20;

        private readonly ApplicationDbContext _context;

        public PlateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlateListResponse> GetAllAsync(QueryOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var skipAmount = options.PageNumber > 1 ? (options.PageNumber -1) * PageSize : 0;

            var totalPlates = _context.Plates.Count();

            var plates = await _context.Plates
                .OrderBy(p => p.Registration)
                .Skip(skipAmount)
                .Take(PageSize)
                .Select(p => new Plate
                {
                    Id = p.Id,
                    Registration = p.Registration,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice * SalesPriceMultiplier,
                    Letters = p.Letters,
                    Numbers = p.Numbers
                })
                .ToListAsync();

            var currentPage = options.PageNumber;
            var totalPages = (int)Math.Ceiling((double)totalPlates / PageSize);

            return new PlateListResponse
            {
                Plates = plates,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                HasNext = currentPage < totalPages,
                HasPrevious = currentPage > 1,
            };
        }

        public async Task AddAsync(Plate plate)
        {
            if (plate.Id == Guid.Empty)
            {
                plate.Id = Guid.NewGuid();
            }

            _context.Plates.Add(plate);
            await _context.SaveChangesAsync();
        }

        public async Task<Plate?> GetByIdAsync(Guid id) => await _context.Plates.FindAsync(id); 
    }
}
