using Catalog.DTO;

namespace WebMVC.Services
{
    public interface IPlatesService
    {
        Task<PlateListResponse> GetPlatesAsync(int pageNumber = 1);

        Task<CreateUpdateResult> CreatePlateAsync(PlateCreateRequest plate);
    }
}
