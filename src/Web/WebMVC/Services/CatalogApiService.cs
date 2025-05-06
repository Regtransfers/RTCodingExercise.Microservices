using Catalog.Domain;
using Refit;

namespace WebMVC.Services
{

    public interface ICatalogApiService
    {
        [Get("/PlatesManagementApi/Plates")]
        Task<IEnumerable<Plate>> Plates();

        [Post("/PlatesManagementApi/Plate")]
        Task<bool> AddPlate(Plate plate);

        [Post("/PlatesManagementApi/ReservePlate")]
        Task<bool> ReservePlate(Guid plateId, string status);
    }
}
