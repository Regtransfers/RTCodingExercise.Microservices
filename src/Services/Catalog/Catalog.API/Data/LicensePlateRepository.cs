
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.API.Data
{
    public class LicensePlateRepository : ILicensePlateRepository
    {
        private readonly ApplicationDbContext _context;

        public LicensePlateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plate>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task AddLicensePlateAsync(Plate plate)
        {
            throw new NotImplementedException();
        }
    }
}
