using Catalog.Domain;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PlatesController : Controller
    {
        private readonly ICatalogApiService _catalogApiService;
        private readonly IConfiguration _configuration;

        public PlatesController(ICatalogApiService catalogApiService, IConfiguration configuration)
        {
            _catalogApiService = catalogApiService;
            _configuration = configuration;
        }

        public async Task<ViewResult> Index(int page = 1, int pageSize = 20, string sortBy = "Registration", string sortOrder = "asc", string filter = "")
        {
            var model = await LoadAndFilterPlates(page, pageSize, sortBy, sortOrder, filter);

            //await PlateStatus(model);

            return View(model);
        }

        public async Task<IActionResult> AddPlate(Plate plate)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            var success = await _catalogApiService.AddPlate(plate);

            if (!success)
                return StatusCode(500, "Failed to add plate");

            return Ok();
        }

        [HttpGet]
        public async Task<PartialViewResult> PlatesTable(int page = 1, int pageSize = 20, string sortBy = "Registration", string sortOrder = "asc", string filter = "")
        {
            var model = await LoadAndFilterPlates(page, pageSize, sortBy, sortOrder, filter);

            //await PlateStatus(model);

            return PartialView("_PlatesTable", model);
        }

        //private async Task PlateStatus(PaginatedPlatesViewModel model)
        //{
        //    foreach (var plate in model.Plates)
        //    { 
        //        plate.Status = plate.Status is "Reserved" ? "Reserved" : "Unreserved";
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> ReservePlate(Guid id, string status)
        {
            switch (status)
            {
                case "Unreserved":
                    status = "Reserved";
                    break;
                case "Reserved":
                    status = "Unreserved";
                    break;
                default:
                    status = status;
                    break;
            }

            await _catalogApiService.ReservePlate(id, status);

            var model = await LoadAndFilterPlates();//temp to get working

            //await PlateStatus(model);

            return PartialView("_PlatesTable", model);
        }

        private async Task<PaginatedPlatesViewModel> LoadAndFilterPlates(int page = 1, int pageSize = 20, string sortBy = "Registration", string sortOrder = "asc", string filter = "")
        {
            var totalPlates = await _catalogApiService.Plates();
            var salesMultiplier = decimal.TryParse(_configuration["SalesMultiplier"], out var parsedMultiplier) ? parsedMultiplier : 1m;

            var platesList = totalPlates.ToList();//NOT PENDING

            // Filter
            if (!string.IsNullOrWhiteSpace(filter))
            {
                platesList = platesList.Where(p => p.Registration.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sorting
            switch (sortBy)
            {
                case "PurchasePrice":
                    platesList = sortOrder == "asc"
                        ? platesList.OrderBy(p => p.PurchasePrice).ToList()
                        : platesList.OrderByDescending(p => p.PurchasePrice).ToList();
                    break;
                case "SalePrice":
                    platesList = sortOrder == "asc"
                        ? platesList.OrderBy(p => p.SalePrice).ToList()
                        : platesList.OrderByDescending(p => p.SalePrice).ToList();
                    break;
                default:
                    platesList = platesList.OrderBy(p => p.Registration).ToList();
                    break;
            }

            var plates = platesList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            var viewPlates = new List<ViewPlate>();
            foreach (var plate in plates)
            {
                viewPlates.Add(new ViewPlate
                {
                    Id = plate.Id,
                    Registration = plate.Registration,
                    PurchasePrice = plate.PurchasePrice,
                    SalePrice = plate.SalePrice,
                    Letters = plate.Letters,
                    Numbers = plate.Numbers,
                    Status = plate.Status ?? "Unreserved"
                });
            }


            var model = new PaginatedPlatesViewModel
            {
                Plates = viewPlates,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)platesList.Count() / pageSize),
                SalesMultiplier = salesMultiplier,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Filter = filter
            };

            //await PlateStatus(model);


            return model;
        }

    }
}