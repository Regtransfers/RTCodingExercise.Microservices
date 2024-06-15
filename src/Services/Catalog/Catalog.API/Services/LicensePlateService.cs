
namespace Catalog.API.Services
{
    public class LicensePlateService : ILicensePlateService
    {
        public readonly ILicensePlateRepository _licensePlateRepository;

        public LicensePlateService(ILicensePlateRepository licensePlateRepository)
        {
            _licensePlateRepository = licensePlateRepository;
        }

        public async Task<IEnumerable<Plate>> GetAllAsync() => await _licensePlateRepository.GetAllAsync();

        public async Task AddLicensePlate(Plate plate) => await _licensePlateRepository.AddLicensePlateAsync(plate);
    }
}
