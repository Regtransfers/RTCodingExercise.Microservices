namespace Catalog.API.Controllers;
//could implement oauth here to secure the api, but havent to save time for now
[ApiController]
[Route("SellingApi")]
//[Authorize("Admin")]//needs oauth setting up and roles etc admin readonly etc
public class SellingApiController : Controller
{
    private readonly ApplicationDbContext _context;

    public SellingApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("Sell")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<bool> Sell(Plate plate)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 2s, 4s, 8s
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                });

        try
        {
            await using (_context)
            {
                var reservedPlateToUpdate = _context.Plates.FirstOrDefault(i => i.Id == plate.Id);
                if (reservedPlateToUpdate != null)
                {
                    reservedPlateToUpdate.Status = "Sold";
                }

                await retryPolicy.ExecuteAsync(async () => { await _context.SaveChangesAsync(); });
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Final failure after retries: {e}");
            return false;
        }
    }


}