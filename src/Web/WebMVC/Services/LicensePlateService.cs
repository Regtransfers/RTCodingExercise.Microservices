using System.Text.Json;

namespace WebMVC.Services
{
    public class LicensePlateService : ILicensePlateService
    {
        private readonly HttpClient _httpClient;

        public LicensePlateService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IEnumerable<Plate>> GetPlatesAsync()
        {
            var response = await _httpClient.GetAsync("http://catalog-api:80/api/licenseplate");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var plates = JsonSerializer.Deserialize<List<Plate>>(content);

            if (plates != null && plates.Any())
                return plates.Select(x => { x.SalePrice = x.SalePrice * 1.2M; return x; }).ToList();

            return new List<Plate>();
        }

        public async Task AddLicensePlate(Plate model)
        {
            var plate = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync("http://catalog-api:80/api/licenseplate", new StringContent(plate));
            response.EnsureSuccessStatusCode();
        }
    }
}
