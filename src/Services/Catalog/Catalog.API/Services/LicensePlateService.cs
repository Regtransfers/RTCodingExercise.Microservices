
namespace Catalog.API.Services
{
    public class LicensePlateService : ILicensePlateService
    {
        public readonly ILicensePlateRepository _licensePlateRepository;

        public LicensePlateService(ILicensePlateRepository licensePlateRepository)
        {
            _licensePlateRepository = licensePlateRepository;
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
