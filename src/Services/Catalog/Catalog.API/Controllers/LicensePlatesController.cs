using Catalog.API.Services;

namespace Catalog.API.Controllers
{
    [Route("api/licenseplate")]
    [ApiController]
    public class LicensePlatesController : ControllerBase
    {
        private readonly ILicensePlateService _licensePlatesService;
        private readonly ILogger<LicensePlatesController> _logger;

        public LicensePlatesController(ILicensePlateService platesService, ILogger<LicensePlatesController> logger)
        {
            _licensePlatesService = platesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlatesAsync()
        {
            try
            {
                var plates = await _licensePlatesService.GetAllAsync();
                return Ok(plates);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retriving license plates - {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLicensePlate([FromBody] Plate plate)
        {
            try
            {
                if (!ModelState.IsValid)
                { 
                    _logger.LogError($"Error add new license plate requested plate is not valid.");
                    return BadRequest(ModelState);
                }

                await _licensePlatesService.AddLicensePlate(plate);

                _logger.LogInformation($"Successfully added license plate {plate.Registration}.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new license plate - {ex.Message}.");
                return BadRequest(ex);
            }
        }
    }
}
