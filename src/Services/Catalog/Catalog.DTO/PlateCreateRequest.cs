using System.ComponentModel.DataAnnotations;

namespace Catalog.DTO
{
    public class PlateCreateRequest
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]{1,7}$", ErrorMessage = $"{nameof(Registration)} should be only numbers and letters and must be 7 characters or less.")]
        public string Registration { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = $"{nameof(PurchasePrice)} must be zero or greater.")]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = $"{nameof(SalePrice)} must be zero or greater.")]
        public decimal SalePrice { get; set; }

        [RegularExpression("^[a-zA-Z]{1,3}$", ErrorMessage = $"{nameof(Letters)} should be only letters and must be 3 characters or less.")]
        public string Letters { get; set; }

        [Range(0, 999)]
        public int Numbers { get; set; }
    }
}
