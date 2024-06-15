namespace Catalog.API.Data
{
    public class LicensePlateRepository : ILicensePlateRepository
    {
        private readonly ApplicationDbContext _context;

        public LicensePlateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plate>> GetAllAsync() => await _context.Plates.ToListAsync();

        public async Task AddLicensePlateAsync(Plate plate)
        {
            _context.Plates.Add(plate);
            await _context.SaveChangesAsync();
        }
    }
}
