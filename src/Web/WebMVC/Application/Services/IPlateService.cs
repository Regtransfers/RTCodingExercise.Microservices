using WebMVC.Application.DTO;

namespace WebMVC.Application.Services;

public interface IPlateService
{
    Task<IEnumerable<PlateDto>> GetAllAsync();
    Task<PaginatedList<PlateDto>> GetPagedPlatesAsync(int pageNumber, int pageSize);
}