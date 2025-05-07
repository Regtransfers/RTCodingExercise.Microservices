using WebMVC.Models;
using WebMVC.Models.Messages;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICatalogApiService _catalogApiService;
        private readonly IConfiguration _configuration;
        private readonly IMessageService _messageService;

        public ShopController(ICatalogApiService catalogApiService, IConfiguration configuration, IMessageService messageService)
        {
            _catalogApiService = catalogApiService;
            _configuration = configuration;
            _messageService = messageService;
        }

        public async Task<ViewResult> Index()
        {
            var model = await LoadAndFilterPlates();

            return View(model);
        }

        private async Task<ShopViewModel> LoadAndFilterPlates()
        {
            var totalPlates = await _catalogApiService.Plates();
            var salesMultiplier = decimal.TryParse(_configuration["SalesMultiplier"], out var parsedMultiplier) ? parsedMultiplier : 1m;

            var platesList = totalPlates.ToList().Where(x => x.Status != "Pending" && x.Status != "Sold" && x.Status != "Reserved");
            
            var model = new ShopViewModel
            {
                Plates = platesList,
                SalesMultiplier = salesMultiplier,
            };
            
            return model;
        }

        [HttpPost]
        public async Task<IActionResult> BuyPlate(Guid id)
        {
            await _messageService.SendMessage<IPendingPlate>(new PendingPlate
            {
                PlateId = id,
                Timestamp = DateTime.UtcNow
            }, "pendingplate-queue");

            var model = await LoadAndFilterPlates();//temp to get working

            return PartialView("_ShopTable", model);
        }
    }
}
