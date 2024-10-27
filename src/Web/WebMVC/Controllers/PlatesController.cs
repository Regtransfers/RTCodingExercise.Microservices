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

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var platesResponse = await _platesService.GetPlatesAsync(pageNumber);

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

                TempData["Foo"] = "xyz";
                TempData["ErrorDetails"] = createResult.ErrorMessage;
                return View(plate);
            }

            return View(plate);
        }
    }
}
