using RTCodingExercise.Microservices.Models;
using System.Diagnostics;
using Catalog.Domain;
using System.Net.Http.Json;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string? orderBy, string? filter)
        {
            var client = _httpClientFactory.CreateClient();
            var url = "http://localhost:5101/api/plates";
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(orderBy)) query.Add($"orderBy={orderBy}");
            if (!string.IsNullOrWhiteSpace(filter)) query.Add($"filter={filter}");
            if (query.Any()) url += "?" + string.Join("&", query);

            var plates = await client.GetFromJsonAsync<List<Plate>>(url) ?? new List<Plate>();
            return View(plates);
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