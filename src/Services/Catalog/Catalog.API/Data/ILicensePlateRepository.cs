namespace Catalog.API.Data
{
    public interface ILicensePlateRepository
    {
        Task<IEnumerable<Plate>> GetAllAsync();

        Task AddLicensePlateAsync(Plate plate);
    }
}
