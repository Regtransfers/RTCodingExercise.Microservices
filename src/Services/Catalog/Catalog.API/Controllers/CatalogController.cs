using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CatalogController> _logger;
        private readonly IDistributedCache _cache;
        private const decimal MarkupPercentage = 1.2M;
        private const decimal DiscountFlat = 25M;
        private const decimal DiscountPercentage = 0.85M;
        private const decimal MinSalePricePercentage = 0.9M;

        public CatalogController(ApplicationDbContext context, ILogger<CatalogController> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlates(int pageNumber = 1, int pageSize = 20)
        {
            var plates = await _context.Plates
                .OrderBy(p => p.Plate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Plate,
                    p.PurchasePrice,
                    SalePrice = p.PurchasePrice * MarkupPercentage
                })
                .ToListAsync();
            return Ok(plates);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate([FromBody] Plate plate)
        {
            _context.Plates.Add(plate);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlates), new { id = plate.Id }, plate);
        }

        [HttpGet("ordered-by-price")]
        public async Task<IActionResult> GetPlatesOrderedByPrice()
        {
            var plates = await _context.Plates
                .OrderBy(p => p.PurchasePrice * MarkupPercentage)
                .Select(p => new
                {
                    p.Plate,
                    p.PurchasePrice,
                    SalePrice = p.PurchasePrice * MarkupPercentage
                })
                .ToListAsync();
            return Ok(plates);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterPlates(string query)
        {
            var plates = await _context.Plates
                .Where(p => p.Plate.Contains(query))
                .Select(p => new
                {
                    p.Plate,
                    p.PurchasePrice,
                    SalePrice = p.PurchasePrice * MarkupPercentage
                })
                .ToListAsync();
            return Ok(plates);
        }

        [HttpPost("reserve/{id}")]
        public async Task<IActionResult> ReservePlate(int id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null)
            {
                _logger.LogWarning("Plate {PlateId} not found for reservation at {Time}", id, DateTime.UtcNow);
                return NotFound("Plate not found.");
            }
            if (plate.IsSold)
            {
                _logger.LogWarning("Attempt to reserve already sold plate {PlateId} at {Time}", id, DateTime.UtcNow);
                return BadRequest("Cannot reserve a sold plate.");
            }

            plate.IsReserved = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Plate {PlateId} reserved at {Time}", plate.Id, DateTime.UtcNow);

            return NoContent();
        }

        [HttpGet("filter-for-sale")]
        public async Task<IActionResult> FilterPlatesForSale(string query)
        {
            var cacheKey = $"plates_{query}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
                return Ok(JsonConvert.DeserializeObject<List<Plate>>(cachedData));

            var plates = await _context.Plates
                .Where(p => p.Plate.Contains(query) && !p.IsReserved)
                .Select(p => new
                {
                    p.Plate,
                    p.PurchasePrice,
                    SalePrice = p.PurchasePrice * MarkupPercentage
                })
                .ToListAsync();

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(plates), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return Ok(plates);
        }

        [HttpPost("sell/{id}")]
        public async Task<IActionResult> SellPlate(int id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null)
            {
                _logger.LogWarning("Plate {PlateId} not found for sale at {Time}", id, DateTime.UtcNow);
                return NotFound("Plate not found.");
            }
            if (plate.IsReserved)
            {
                _logger.LogWarning("Attempt to sell reserved plate {PlateId} at {Time}", id, DateTime.UtcNow);
                return BadRequest("Cannot sell a reserved plate.");
            }
            if (plate.IsSold)
            {
                _logger.LogWarning("Attempt to sell already sold plate {PlateId} at {Time}", id, DateTime.UtcNow);
                return BadRequest("Plate is already sold.");
            }

            plate.IsSold = true;
            decimal salePrice = plate.PurchasePrice * MarkupPercentage;
            _context.TotalRevenue += salePrice;

            _context.Sales.Add(new Sale
            {
                PlateId = plate.Id,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = salePrice,
                SaleDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation("Plate {PlateId} sold at {Time} for {Price}", plate.Id, DateTime.UtcNow, salePrice);

            return NoContent();
        }

        [HttpGet("discount")]
        public async Task<IActionResult> ApplyDiscount(string promoCode)
        {
            if (promoCode != "DISCOUNT" && promoCode != "PERCENTOFF")
                return BadRequest("Invalid promo code.");

            var plates = await _context.Plates
                .Where(p => !p.IsReserved && !p.IsSold)
                .Select(p => new
                {
                    p.Plate,
                    p.PurchasePrice,
                    SalePrice = CalculateDiscountedPrice(p.PurchasePrice, promoCode, MinSalePricePercentage)
                })
                .ToListAsync();
            return Ok(plates);
        }

        private decimal CalculateDiscountedPrice(decimal purchasePrice, string promoCode, decimal minPercentage)
        {
            var salePrice = purchasePrice * MarkupPercentage;
            var discountedPrice = promoCode switch
            {
                "DISCOUNT" => salePrice - DiscountFlat,
                "PERCENTOFF" => salePrice * DiscountPercentage,
                _ => salePrice
            };

            return discountedPrice < salePrice * minPercentage ? salePrice : discountedPrice;
        }
    }
}
