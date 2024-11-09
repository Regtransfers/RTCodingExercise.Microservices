using System.Diagnostics;
using RTCodingExercise.Microservices.Models;
using WebMVC.Application.Services;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlateService _plateService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IPlateService plateService, ILogger<HomeController> logger)
        {
            _plateService = plateService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var paginatedPlates = await _plateService.GetPagedPlatesAsync(pageNumber, 20);
            var model = new Home
            {
                Plates = paginatedPlates.Items,
                PageNumber = paginatedPlates.PageNumber,
                HasPreviousPage = paginatedPlates.HasPreviousPage,
                HasNextPage = paginatedPlates.HasNextPage
            };
    
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}