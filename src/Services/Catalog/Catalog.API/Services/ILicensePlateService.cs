namespace Catalog.API.Services
{
    public interface ILicensePlateService
    {
        Task<IEnumerable<Plate>> GetAllAsync();

        Task AddLicensePlate(Plate plate);
    }
}
