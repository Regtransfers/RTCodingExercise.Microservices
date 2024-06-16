namespace WebMVC.Services
{
    public interface ILicensePlateService
    {
        Task<IEnumerable<Plate>> GetPlatesAsync();
        Task AddLicensePlate(Plate model);
    }
}
