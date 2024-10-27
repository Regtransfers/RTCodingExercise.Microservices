using Catalog.API.Requests;
using Catalog.DTO;

namespace Catalog.API.Data
{
    public interface IPlateRepository
    {
        Task<Plate?> GetByIdAsync(Guid id);

        Task<PlateListResponse> GetAllAsync(QueryOptions options);

        Task AddAsync(Plate plate);
    }
}
