using Catalog.DTO;

namespace WebMVC.Services
{
    public class PlatesService : IPlatesService
    {
        private readonly HttpClient _httpClient;

        public PlatesService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PlatesApi");
        }

        public async Task<PlateListResponse> GetPlatesAsync(int pageNumber = 1)
        {
            var platesResponse = await _httpClient.GetFromJsonAsync<PlateListResponse>($"api/plates?pageNumber={pageNumber}");

            return platesResponse;
        }

        public async Task<CreateUpdateResult> CreatePlateAsync(PlateCreateRequest plate)
        {
            var createResponse = await _httpClient.PostAsJsonAsync("api/plates", plate);

            if (createResponse.IsSuccessStatusCode)
            {
                return new CreateUpdateResult
                {
                    IsSuccess = true
                };
            }

            return new CreateUpdateResult
            { 
                IsSuccess = false,
                ErrorMessage = createResponse.ReasonPhrase
            };
        }
    }
}
