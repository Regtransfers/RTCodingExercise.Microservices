using WebMVC.Application.DTO;
using WebMVC.Infrastructure.Data.Repositories;

namespace WebMVC.Application.Services;

public sealed class PlateService : IPlateService
{
    private readonly IPlateRepository _plateRepository;

    public PlateService(IPlateRepository plateRepository)
    {
        _plateRepository = plateRepository;
    }

    public async Task<IEnumerable<PlateDto>> GetAllAsync()
    {
        var plates = await _plateRepository.GetAllAsync();

        var plateDtos = plates.Select(plate => new PlateDto
        {
            Id = plate.Id,
            Registration = plate.Registration,
            PurchasePrice = plate.PurchasePrice,
            SalePrice = plate.SalePrice.Value,
            Letters = plate.Letters,
            Numbers = plate.Numbers
        });

        return plateDtos;
    }

    public async Task<PaginatedList<PlateDto>> GetPagedPlatesAsync(int pageNumber, int pageSize)
    {
        var pagedPlates = await _plateRepository.GetPaginatedAsync(pageNumber, pageSize);
        
        return new PaginatedList<PlateDto>(
            pagedPlates.Select(plate => new PlateDto
            {
                Id = plate.Id,
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice.Value,
                Letters = plate.Letters,
                Numbers = plate.Numbers
            }).ToList(),
            pageNumber,
            pageSize
        );
    }
}