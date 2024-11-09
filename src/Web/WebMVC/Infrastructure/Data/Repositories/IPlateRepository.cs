using Catalog.Domain.Entities;

namespace WebMVC.Infrastructure.Data.Repositories;

public interface IPlateRepository
{
    Task<IEnumerable<Plate>> GetAllAsync();
    Task<IEnumerable<Plate>> GetPaginatedAsync(int pageNumber, int pageSize);
}