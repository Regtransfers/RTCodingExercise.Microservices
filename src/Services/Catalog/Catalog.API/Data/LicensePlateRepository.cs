
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.API.Data
{
    public class LicensePlateRepository : ILicensePlateRepository
    {
        private readonly IDesignTimeDbContextFactory<ApplicationDbContext> _contextFactory;

        public LicensePlateRepository(IDesignTimeDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Plate>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task AddLicensePlate(Plate plate)
        {
            throw new NotImplementedException();
        }
    }
}
