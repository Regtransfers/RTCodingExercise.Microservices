using MassTransit;
using WebMVC.Models.Messages;

namespace WebMVC.Services
{
    public interface IShopService
    {
        Task<bool> SellPlate(Guid plateId);
    }

    public class ShopService : IShopService
    {
        private readonly ICatalogApiService _catalogApiService;

        public ShopService(ICatalogApiService catalogApiService)
        {
            _catalogApiService = catalogApiService;
        }

        public async Task<bool> SellPlate(Guid plateId)
        {
            if (plateId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(plateId));
            }

            return await _catalogApiService.SellPlate(plateId);
        }
    }

    public class PendingPlateConsumer : IConsumer<IPendingPlate>
    {
        private readonly IShopService _shopService;

        public PendingPlateConsumer(IShopService shopService)
        {
            _shopService = shopService;
        }

        public async Task Consume(ConsumeContext<IPendingPlate> context)
        {
            await _shopService.SellPlate(context.Message.PlateId);
        }
    }

}
