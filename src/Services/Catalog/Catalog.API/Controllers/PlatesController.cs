using Catalog.API.Data;
using Catalog.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/plates?orderBy=price&filter=abc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plate>>> Get([FromQuery] string? orderBy, [FromQuery] string? filter)
        {
            IQueryable<Plate> query = _context.Plates.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.Registration!.Contains(filter) ||
                                         p.Letters!.Contains(filter) ||
                                         p.Numbers.ToString().Contains(filter));
            }

            if (string.Equals(orderBy, "price", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(p => p.SalePrice);
            }

            var plates = await query.ToListAsync();

            // apply 20% markup
            foreach (var plate in plates)
            {
                plate.SalePrice = Math.Round(plate.SalePrice * 1.2m, 2);
            }

            return Ok(plates);
        }

        // POST api/plates/{id}/reserve
        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> Reserve(Guid id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null) return NotFound();

            plate.Status = PlateStatus.Reserved;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST api/plates/{id}/sell
        [HttpPost("{id}/sell")]
        public async Task<IActionResult> Sell(Guid id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null) return NotFound();

            plate.Status = PlateStatus.Sold;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
