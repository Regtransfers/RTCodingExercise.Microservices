using Catalog.API.Requests;
using Catalog.DTO;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatesController : ControllerBase
    {
        private readonly IPlateRepository _repository;

        public PlatesController(IPlateRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plate>> GetPlateById(Guid id)
        {
            // only exists for the purpose of route generation in 'Create'.
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult<PlateListResponse>> GetAll([FromQuery] QueryOptions? options)
        {
            options ??= new QueryOptions { PageNumber = 1 };

            if (options.PageNumber < 1)
            {
                options.PageNumber = 1;
            }

            return await _repository.GetAllAsync(options);
        }

        [HttpPost]
        public async Task<ActionResult<Plate>> Create([FromBody] PlateCreateRequest request)
        {
            var plateModel = new Plate
            {
                Id = Guid.NewGuid(),
                Registration = request.Registration,
                PurchasePrice = request.PurchasePrice,
                SalePrice = request.SalePrice,
                Letters = request.Letters,
                Numbers = request.Numbers,
            };

            await _repository.AddAsync(plateModel);

            return CreatedAtAction(nameof(GetPlateById), new { id = plateModel.Id }, plateModel);
        }
    }
}
