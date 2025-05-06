using Newtonsoft.Json;

//could implement oauth here to secure the api, but havent to save time for now
namespace Catalog.API.Controllers;

[ApiController]
[Route("PlatesManagementApi")]
//[Authorize("Admin")]//needs oauth setting up and roles etc admin readonly etc
public class PlatesApiController : Controller
{
    private readonly ApplicationDbContext _context;

    public PlatesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("Plates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<string> Plates()
    {
        var plates = _context.Plates.ToList(); // fetches all Plate records from the DB
        return JsonConvert.SerializeObject(plates);
    }

    [HttpPost("Plate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<bool> Plate(Plate plate)
    {
        try
        {
            await _context.Plates.AddAsync(plate);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    [HttpPost("ReservePlate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<bool> ReservePlate(Guid plateId, string status)
    {
        try
        {
            await using (_context)
            {
                var reservedPlateToUpdate = _context.Plates.FirstOrDefault(i => i.Id == plateId);
                if (reservedPlateToUpdate != null)
                {
                    reservedPlateToUpdate.Status = status;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}