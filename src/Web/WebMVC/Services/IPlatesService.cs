using Catalog.DTO;

namespace WebMVC.Services
{
    public interface IPlatesService
    {
        Task<PlateListResponse> GetPlatesAsync(SortOptions orderBy, int pageNumber);

        Task<CreateUpdateResult> CreatePlateAsync(PlateCreateRequest plate);
    }
}
