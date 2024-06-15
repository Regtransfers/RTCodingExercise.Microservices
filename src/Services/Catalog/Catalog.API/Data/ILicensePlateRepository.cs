namespace Catalog.API.Data
{
    public interface ILicensePlateRepository
    {
        Task<IEnumerable<Plate>> GetAllAsync();

        Task AddLicensePlate(Plate plate);
    }
}
