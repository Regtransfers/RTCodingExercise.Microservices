using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class PlatesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlatesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("CatalogAPI");

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                ModelState.AddModelError(string.Empty, "Invalid page number.");
                return View("Error");
            }

            var client = CreateClient();
            try
            {
                var response = await client.GetAsync($"api/catalog?pageNumber={pageNumber}");
                response.EnsureSuccessStatusCode();

                var plates = await response.Content.ReadAsAsync<IEnumerable<PlateViewModel>>();
                ViewData["PageNumber"] = pageNumber;
                return View("Index", plates);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching data.");
                return View("Error");
            }
        }

        public async Task<IActionResult> OrderedByPrice(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                ModelState.AddModelError(string.Empty, "Invalid page number.");
                return View("Error");
            }

            var client = CreateClient();
            try
            {
                var response = await client.GetAsync($"api/catalog/ordered-by-price?pageNumber={pageNumber}");
                response.EnsureSuccessStatusCode();

                var plates = await response.Content.ReadAsAsync<IEnumerable<PlateViewModel>>();
                ViewData["PageNumber"] = pageNumber;
                return View("Index", plates);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while ordering data.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Filter(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ModelState.AddModelError(string.Empty, "Please enter a search query.");
                return RedirectToAction("Index");
            }

            var client = CreateClient();
            try
            {
                var response = await client.GetAsync($"api/catalog/filter?query={System.Net.WebUtility.UrlEncode(query)}");
                response.EnsureSuccessStatusCode();

                var plates = await response.Content.ReadAsAsync<IEnumerable<PlateViewModel>>();
                return View("Index", plates);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while filtering data.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Reserve(int id)
        {
            var client = CreateClient();
            try
            {
                var response = await client.PostAsync($"api/catalog/reserve/{id}", null);
                response.EnsureSuccessStatusCode();

                TempData["Message"] = $"Plate #{id} has been reserved successfully.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while reserving the plate.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Sell(int id)
        {
            var client = CreateClient();
            try
            {
                var response = await client.PostAsync($"api/catalog/sell/{id}", null);
                response.EnsureSuccessStatusCode();

                TempData["Message"] = $"Plate #{id} has been sold successfully.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while selling the plate.");
                return View("Error");
            }
        }

        public async Task<IActionResult> ApplyDiscount(string promoCode)
        {
            if (string.IsNullOrEmpty(promoCode))
            {
                ModelState.AddModelError(string.Empty, "Please enter a valid promo code.");
                return RedirectToAction("Index");
            }

            var client = CreateClient();
            try
            {
                var response = await client.GetAsync($"api/catalog/discount?promoCode={promoCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Invalid promo code or discount not applicable.");
                    return View("Error");
                }

                var plates = await response.Content.ReadAsAsync<IEnumerable<PlateViewModel>>();
                ViewData["AppliedPromoCode"] = promoCode;
                return View("Index", plates);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while applying the discount.");
                return View("Error");
            }
        }
    }
}
