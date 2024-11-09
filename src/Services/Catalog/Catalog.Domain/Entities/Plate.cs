using Catalog.Domain.ValueObjects;

namespace Catalog.Domain.Entities
{
    public class Plate
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        public decimal PurchasePrice { get; set; }

        public SalePrice SalePrice => new(PurchasePrice);
        public string? Letters { get; set; }

        public int Numbers { get; set; }
    }
}