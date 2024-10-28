using Catalog.DTO;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class PlatesController : Controller
    {
        private readonly IPlatesService _platesService;

        public PlatesController(IPlatesService platesService)
        {
            _platesService = platesService;
        }

        public async Task<IActionResult> Index(string orderBy = "", int pageNumber = 1)
        {
            ViewData["CurrentSortOrder"] = orderBy;
            ViewData["RegSort"] = string.IsNullOrEmpty(orderBy) ? "registrationDescending" : "";
            ViewData["SalePriceSort"] = orderBy == "salePriceAscending" ? "salePriceDescending" : "salePriceAscending";

            var platesResponse = await _platesService.GetPlatesAsync(ParseOrderBy(orderBy), pageNumber);

            return View(platesResponse);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlateCreateRequest plate)
        {
            if (ModelState.IsValid)
            {
                var createResult = await _platesService.CreatePlateAsync(plate);

                if (createResult.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorDetails"] = createResult.ErrorMessage;
                return View(plate);
            }

            return View(plate);
        }

        private SortOptions ParseOrderBy(string orderBy)
        {
            return orderBy switch
            {
                "salePriceDescending" => SortOptions.SalePriceDescending,
                "salePriceAscending" => SortOptions.SalePriceAscending,
                "registrationDescending" => SortOptions.RegistrationDescending,
                _ => SortOptions.RegistrationAscending,
            };
        }
    }
}
