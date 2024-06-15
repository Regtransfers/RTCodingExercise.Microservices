using Catalog.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/licenseplate")]
    [ApiController]
    public class LicensePlatesController : ControllerBase
    {
        private readonly ILicensePlateService _licensePlatesService;

        public LicensePlatesController(ILicensePlateService platesService)
        {
            _licensePlatesService = platesService;
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
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLicensePlate([FromBody] Plate plate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _licensePlatesService.AddLicensePlate(plate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
